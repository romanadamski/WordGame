using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DictionarleController : BaseGameController
{
    [SerializeField]
    private EventChannelSO eventChannelSO;

    [SerializeField]
    private GameSettingsSO gameSettingsSO;

    private List<string> guesses;
    private List<string> answers;

    private string wordToAnswer = string.Empty;
    private string wordGuess = string.Empty;

    private int currentTry = 0;

    private const int MAX_TRIES = 15;

    public override void LoadGame()
    {
        guesses = gameStateSO.CurrentLanguageConfig.Guesses.text.Split("\n").ToList();
        answers = gameStateSO.CurrentLanguageConfig.Answers.text.Split("\n").ToList();

        warningTextParent.SetActive(false);
        warningText.text = string.Empty;

        eventChannelSO.OnKeyClick.AddListener(OnKeyClick);

        GetWordToGuess();
        currentTry = 0;
    }

    private void GetWordToGuess()
    {
        wordToAnswer = answers[UnityEngine.Random.Range(0, answers.Count)];
        Debug.Log($"wordToAnswer {wordToAnswer}");
    }

    public override void UnloadGame()
    {
        eventChannelSO.OnKeyClick.RemoveListener(OnKeyClick);
        warningTextParent.SetActive(false);
        warningText.text = string.Empty;
    }

    private void OnKeyClick(CustomKeyCode key)
    {
        if (key.PolishKeyCode != PolishKeyCode.None)
        {
            var polishChar = KeyboardController.ParseToPolish(key.PolishKeyCode);
            AddCharacter(polishChar);
        }
        else
        {
            if (key.KeyCode == KeyCode.Return)
            {
                TrySubmitWord();
            }
            else if (key.KeyCode == KeyCode.Backspace)
            {
                RemoveCharacter();
            }
            else
            {
                AddCharacter((char)key.KeyCode);
            }
        }
    }

    private void TrySubmitWord()
    {
        var word = new string(wordGuess);
        if (!guesses.Any(x => x.Equals(word, StringComparison.CurrentCultureIgnoreCase))
            && !answers.Any(x => x.Equals(word, StringComparison.CurrentCultureIgnoreCase)))
        {
            ShowWarningMessage("Not in word list");
        }
        else if (wordToAnswer.Equals(word, StringComparison.CurrentCultureIgnoreCase))
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

    private void HandleColors()
    {

    }

    private void RemoveCharacter()
    {
        if (wordGuess.Length > 0)
        {
            wordGuess = wordGuess[..^1];
            //guessPanelController.ShowLetter(currentIndex, '\0');
        }
    }

    private void AddCharacter(char character)
    {
        if (currentTry < MAX_TRIES)
        {
            wordGuess += character;
            //guessPanelController.ShowLetter(currentTry, currentIndex, character);
        }
    }
}
