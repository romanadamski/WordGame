using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum GuessType
{
    None = 0,
    Wrong,
    Good1,
    Good2
}

public class LetterData
{
    public int Index;
    public char Character;
    public Color Color;
    public GuessType GuessType;
}

public class WordleController : BaseGameController
{
    [SerializeField]
    private EventChannelSO eventChannelSO;
    
    [SerializeField]
    private GameSettingsSO gameSettingsSO;
    
    [SerializeField]
    private WordleGuessPanelController guessPanelController;
    
    [SerializeField]
    private KeyboardController keyboardController;
    
    private List<string> guesses;
    private List<string> answers;

    private string wordToAnswer = string.Empty;
    private char[] wordGuess = new char[MAX_LETTERS];

    private int currentIndex = 0;
    private int currentTry = 0;

    private const int MAX_TRIES = 6;
    private const int MAX_LETTERS = 5;

    public override void LoadGame()
    {
        guesses = gameStateSO.CurrentLanguageConfig.Guesses.text.Split("\n").ToList();
        answers = gameStateSO.CurrentLanguageConfig.Answers.text.Split("\n").ToList();

        warningTextParent.SetActive(false);
        warningText.text = string.Empty;

        eventChannelSO.OnKeyClick.AddListener(OnKeyClick);
        wordGuess = new char[MAX_LETTERS];;
        GetWordToGuess();
        currentIndex = 0;
        currentTry = 0;
    }

    public override void UnloadGame()
    {
        eventChannelSO.OnKeyClick.RemoveListener(OnKeyClick);
        warningTextParent.SetActive(false);
        warningText.text = string.Empty;
    }

    private void GetWordToGuess()
    {
        wordToAnswer = answers[UnityEngine.Random.Range(0, answers.Count)];
        Debug.Log($"wordToAnswer {wordToAnswer}");
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
        if (currentIndex < MAX_LETTERS)
        {
            ShowWarningMessage("Not enough letters");
        }
        else if (!guesses.Any(x => x.Equals(word, StringComparison.CurrentCultureIgnoreCase))
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

    private Dictionary<char, int> GetLetterCount()
    {
        Dictionary<char, int> letterCount = new();
        foreach (var letter in wordToAnswer)
        {
            if (!letterCount.ContainsKey(letter))
            {
                letterCount.Add(letter, 1);
            }
            else
            {
                letterCount[letter]++;
            }
        }

        return letterCount;
    }

    private void HandleColors()
    {
        var letterCount = GetLetterCount();
        List<LetterData> guess = new();
        
        //First pass = check green ones
        for (int i = 0; i < wordGuess.Length; i++)
        {
            if (wordGuess[i].ToString().Equals(wordToAnswer[i].ToString(), StringComparison.CurrentCultureIgnoreCase))
            {
                guess.Add(new()
                {
                    Index = i,
                    Character = wordGuess[i],
                    Color = gameSettingsSO.GoodColor2,
                    GuessType = GuessType.Good2
                });
                letterCount[wordToAnswer[i]]--;
            }
        }

        //First pass = check rest (only if letterCount > 0)
        for (int i = 0; i < wordGuess.Length; i++)
        {
            var character = wordGuess[i];
            if (!guess.Any(x => x.Index == i))
            {
                if (wordToAnswer.Contains(character)
                    && letterCount[character] > 0)
                {
                    guess.Add(new()
                    {
                        Index = i,
                        Character = character,
                        Color = gameSettingsSO.GoodColor1,
                        GuessType = GuessType.Good1
                    });
                    letterCount[character]--;
                }
                else
                {
                    guess.Add(new()
                    {
                        Index = i,
                        Character = character,
                        Color = gameSettingsSO.WrongColor,
                        GuessType = GuessType.Wrong
                    });
                }
            }
        }

        guessPanelController.SetColors(currentTry, guess);
        keyboardController.SetColors(guess);
        currentTry++;
        currentIndex = 0;
        wordGuess = new char[MAX_LETTERS];
    }

    private void RemoveCharacter()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            wordGuess[currentIndex] = '\0';
            guessPanelController.ShowLetter(currentTry, currentIndex, '\0');
        }
    }

    private void AddCharacter(char character)
    {
        if (currentIndex < MAX_LETTERS && currentTry < MAX_TRIES)
        {
            wordGuess[currentIndex] = character;
            guessPanelController.ShowLetter(currentTry, currentIndex, character);
            currentIndex++;
        }
    }
}
