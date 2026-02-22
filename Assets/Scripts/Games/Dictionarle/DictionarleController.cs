using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DictionarleController : BaseGameController
{
    [SerializeField]
    private EventChannelSO eventChannelSO;

    [SerializeField]
    private GameSettingsSO gameSettingsSO;
    
    [SerializeField]
    private DictionarleGuessController guessPrefab;
    
    [SerializeField]
    private TMP_InputField inputField;
    
    [SerializeField]
    private Transform guessPanel;

    private List<string> guesses;
    private List<string> answers;

    private string wordToAnswer = string.Empty;
    private string wordGuess = string.Empty;

    private int currentTry = 0;

    private readonly List<DictionarleGuessController> guessesBeforeAnswer = new();
    private readonly List<DictionarleGuessController> guessesAfterAnswer = new();
    private StringComparer stringComparer;

    private UnityEvent onSubmit = new();

    private const int MAX_TRIES = 15;
    private const string GREEN_PREFIX = "<color=#35A919>";
    private const string GREEN_POSTFIX = "</color>";

    public override void LoadGame()
    {
        guesses = gameStateSO.CurrentLanguageConfig.Guesses.text.Split("\n").ToList();
        answers = gameStateSO.CurrentLanguageConfig.Answers.text.Split("\n").ToList();

        warningTextParent.SetActive(false);
        warningText.text = string.Empty;

        GetWordToGuess();
        currentTry = 0;

        var cultureInfo = gameStateSO.CurrentLanguageConfig.Language switch
        {
            LanguageType.EN => CultureInfo.GetCultureInfo("en-US"),
            LanguageType.PL => CultureInfo.GetCultureInfo("pl-PL"),
            _ => CultureInfo.GetCultureInfo("en-US"),
        };
        stringComparer = StringComparer.Create(cultureInfo, ignoreCase: true);

        inputField.gameObject.SetActive(true);
        inputField.text = string.Empty;
        inputField.Select();

        onSubmit.AddListener(TrySubmitWord);
        eventChannelSO.OnGameEnd.AddListener(OnGameEnd);
    }

    public override void UnloadGame()
    {
        warningTextParent.SetActive(false);
        warningText.text = string.Empty;
        foreach (var guess in guessesBeforeAnswer)
        {
            Destroy(guess.gameObject);
        }
        foreach (var guess in guessesAfterAnswer)
        {
            Destroy(guess.gameObject);
        }
        guessesBeforeAnswer.Clear();
        guessesAfterAnswer.Clear();
        eventChannelSO.OnGameEnd.RemoveListener(OnGameEnd);
    }

    private void GetWordToGuess()
    {
        wordToAnswer = answers[UnityEngine.Random.Range(0, answers.Count)];
        Debug.Log($"wordToAnswer {wordToAnswer}");
    }

    private void TrySubmitWord()
    {
        wordGuess = inputField.text;
        if (!guesses.Any(x => x.Equals(wordGuess, StringComparison.CurrentCultureIgnoreCase))
            && !answers.Any(x => x.Equals(wordGuess, StringComparison.CurrentCultureIgnoreCase)))
        {
            ShowWarningMessage("Not in word list");
        }
        else if (wordToAnswer.Equals(wordGuess, StringComparison.CurrentCultureIgnoreCase))
        {
            HandleColors();
            ShowWarningMessage("Win!");
            eventChannelSO.OnGameEnd?.Invoke(true, wordToAnswer);
        }
        else if (currentTry < MAX_TRIES)
        {
            HandleColors();
            if (currentTry >= MAX_TRIES)
            {
                ShowWarningMessage("Lose");
                eventChannelSO.OnGameEnd?.Invoke(false, wordToAnswer);
            }
        }
    }

    private void OnGameEnd(bool _, string __)
    {
        onSubmit.RemoveListener(TrySubmitWord);
        inputField.ReleaseSelection();
        inputField.gameObject.SetActive(false);
    }

    private void HandleColors()
    {
        // Win
        if (stringComparer.Compare(wordToAnswer, wordGuess) == 0)
        {
            inputField.text = $"{GREEN_PREFIX}{wordToAnswer}{GREEN_POSTFIX}";
        }
        else
        {
            var guessText = Instantiate(guessPrefab, guessPanel);
            int i = 0;
            for (i = 0; i < wordGuess.Length; i++)
            {
                if (wordToAnswer.Length <= i || stringComparer.Compare(wordGuess[i].ToString(), wordToAnswer[i].ToString()) != 0)
                {
                    break;
                }
            }
            //Any letter guessed
            if (i > 0)
            {
                var restWord = "";
                if (wordGuess.Length <= i && wordGuess.Length < wordToAnswer.Length)
                {
                    restWord = $"...";
                }
                else
                {
                    restWord = wordGuess[i..];
                }
                guessText.GuessLabel.text = $"{GREEN_PREFIX}{wordGuess[..i]}{GREEN_POSTFIX}{restWord}";
            }
            // No letters guessed
            else
            {
                guessText.GuessLabel.text = wordGuess;
            }
            //Guessed word before answer
            if (stringComparer.Compare(wordGuess, wordToAnswer) < 0)
            {
                if (!guessesBeforeAnswer.Any())
                {
                    guessText.transform.SetSiblingIndex(0);
                }
                else
                {
                    var sortedGuesses = guessesBeforeAnswer.OrderBy(x => x.Text, stringComparer).ToList();
                    var inserted = false;
                    for (i = 0; i < sortedGuesses.Count; i++)
                    {
                        if (stringComparer.Compare(wordGuess, sortedGuesses[i].Text) < 0)
                        {
                            guessText.transform.SetSiblingIndex(sortedGuesses[i].transform.GetSiblingIndex());
                            inserted = true;
                            break;
                        }
                    }
                    if (!inserted)
                    {
                        guessText.transform.SetSiblingIndex(inputField.transform.GetSiblingIndex());
                    }
                }
                guessesBeforeAnswer.Add(guessText);
            }
            //Guessed word after answer
            else
            {
                if (!guessesAfterAnswer.Any())
                {
                    guessText.transform.SetSiblingIndex(inputField.transform.GetSiblingIndex() + 1);
                }
                else
                {
                    var sortedGuesses = guessesAfterAnswer.OrderBy(x => x.Text, stringComparer).ToList();
                    var inserted = false;
                    for (i = 0; i < sortedGuesses.Count; i++)
                    {
                        if (stringComparer.Compare(wordGuess, sortedGuesses[i].Text) < 0)
                        {
                            guessText.transform.SetSiblingIndex(sortedGuesses[i].transform.GetSiblingIndex());
                            inserted = true;
                            break;
                        }
                    }
                    if (!inserted)
                    {
                        guessText.transform.SetSiblingIndex(i + 1);
                    }
                }
                guessesAfterAnswer.Add(guessText);
            }
            guessText.Text = wordGuess;
            inputField.text = string.Empty;
        }

        inputField.Select();
    }

    private void Update()
    {
        if(Keyboard.current.enterKey.wasPressedThisFrame)
        {
            onSubmit?.Invoke();
        }
    }
}
