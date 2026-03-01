using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TMPro;
using UnityEngine;

public class DictionarleController : BaseGameController
{
    [SerializeField]
    private EventChannelSO eventChannelSO;

    [SerializeField]
    private GameSettingsSO gameSettingsSO;
    
    [SerializeField]
    private DictionarleGuessLabel guessPrefab;
    
    [SerializeField]
    private PersistentTMPInputField inputField;
    
    [SerializeField]
    private DictionarleGuessesCounter guessCounter;
    
    [SerializeField]
    private Transform guessesBefore;
    
    [SerializeField]
    private Transform guessesAfter;

    private List<string> guesses;
    private List<string> answers;
    //todo dict for dictionarle, proper dict for pl wordle
    private string wordToAnswer = string.Empty;
    private string wordGuess = string.Empty;

    private int currentTry = 0;

    private readonly List<DictionarleGuessLabel> guessesBeforeAnswer = new();
    private readonly List<DictionarleGuessLabel> guessesAfterAnswer = new();
    private StringComparer stringComparer;

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
        inputField.interactable = true;
        inputField.text = string.Empty;
        inputField.onSubmit.AddListener(TrySubmitWord);
        inputField.onValidateInput += (text, charIndex, addedChar) =>
        {
            return char.IsLetter(addedChar) ? addedChar : '\0';
        };
        inputField.ActivateInputField();

        eventChannelSO.OnGameEnd.AddListener(OnGameEnd);

        guessCounter.Init(MAX_TRIES);
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
        inputField.onSubmit.RemoveListener(TrySubmitWord);
    }

    private void GetWordToGuess()
    {
        wordToAnswer = answers[UnityEngine.Random.Range(0, answers.Count)];
        Debug.Log($"wordToAnswer {wordToAnswer}");
    }

    private void TrySubmitWord(string _)
    {
        wordGuess = inputField.text;
        if (!guesses.Any(x => x.Equals(wordGuess, StringComparison.CurrentCultureIgnoreCase))
            && !answers.Any(x => x.Equals(wordGuess, StringComparison.CurrentCultureIgnoreCase)))
        {
            ShowWarningMessage("Not in word list");
            ReactivateInputField();
        }
        else if (wordToAnswer.Equals(wordGuess, StringComparison.CurrentCultureIgnoreCase))
        {
            HandleColors();
            ShowWarningMessage("Win!");
            guessCounter.Win();
            eventChannelSO.OnGameEnd?.Invoke(true, wordToAnswer);
        }
        else if (currentTry < MAX_TRIES)
        {
            HandleColors();
            guessCounter.MoveToNextNumber();
            currentTry++;
            if (currentTry >= MAX_TRIES)
            {
                ShowWarningMessage("Lose");
                eventChannelSO.OnGameEnd?.Invoke(false, wordToAnswer);
                inputField.gameObject.SetActive(false);
            }
            else
            {
                ReactivateInputField();
            }
        }
    }
    //todo not hide to show again
    private void ReactivateInputField()
    {
        inputField.ActivateInputField();
        inputField.Select();

        //StartCoroutine(MoveTextEnd_NextFrame());
    }

    IEnumerator MoveTextEnd_NextFrame()
    {
        yield return null;
        yield return null;
        inputField.MoveTextEnd(false);
    }

    private void OnGameEnd(bool _, string __)
    {
        inputField.DeactivateInputField();
        inputField.interactable = false;
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
            var guessText = Instantiate(guessPrefab, stringComparer.Compare(wordGuess, wordToAnswer) < 0 ? guessesBefore : guessesAfter);
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
                    var sortedGuesses = guessesBeforeAnswer.OrderBy(x => x.transform.GetSiblingIndex()).ToList();
                    var inserted = false;
                    for (i = 0; i < sortedGuesses.Count; i++)
                    {
                        if (stringComparer.Compare(wordGuess, sortedGuesses[i].Text) < 0)
                        {
                            var siblingIndex = sortedGuesses[i].transform.GetSiblingIndex();
                            guessText.transform.SetSiblingIndex(sortedGuesses[i].transform.GetSiblingIndex());
                            inserted = true;
                            break;
                        }
                    }
                    if (!inserted)
                    {
                        guessText.transform.SetSiblingIndex(i);
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
                    var sortedGuesses = guessesAfterAnswer.OrderBy(x => x.transform.GetSiblingIndex()).ToList();
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
                        guessText.transform.SetSiblingIndex(i);
                    }
                }
                guessesAfterAnswer.Add(guessText);
            }
            guessText.Text = wordGuess;
            guessText.name = wordGuess;
            inputField.text = string.Empty;
        }
    }
}
