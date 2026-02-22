using System.Linq;
using UnityEngine;

public class WordleGuessLineController : MonoBehaviour
{
    private WordleGuessLetterController[] letters;
    private Color defaultColor;

    private void Awake()
    {
        letters = GetComponentsInChildren<WordleGuessLetterController>();
        defaultColor = letters.First().Background.color;
    }
    
    public void Clear()
    {
        foreach (var letter in letters)
        {
            letter.Letter.text = "\0";
            letter.Background.color = defaultColor;
        }
    }

    public void ShowLetter(int letterIndex, char character)
    {
        letters[letterIndex].Letter.text = character.ToString().ToUpper();
    }

    public void SetLetterColor(int letterIndex, Color color)
    {
        letters[letterIndex].Background.color = color;
    }
}
