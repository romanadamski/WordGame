using System.Collections.Generic;
using UnityEngine;

public class WordleGuessPanelController : MonoBehaviour
{
    [SerializeField]
    private EventChannelSO eventChannelSO;

    private WordleGuessLineController[] lines;

    private void Awake()
    {
        lines = GetComponentsInChildren<WordleGuessLineController>();
        eventChannelSO.OnGameplayClear.AddListener(Clear);
    }

    private void Clear()
    {
        foreach (var line in lines)
        {
            line.Clear();
        }
    }

    public void ShowLetter(int lineIndex, int letterIndex, char character)
    {
        var line = lines[lineIndex];
        line.ShowLetter(letterIndex, character);
    }

    public void SetColors(int lineIndex, List<LetterData> lettersColors)
    {
        var line = lines[lineIndex];
        foreach (var lettersColor in lettersColors)
        {
            line.SetLetterColor(lettersColor.Index, lettersColor.Color);
        }
    }
}
