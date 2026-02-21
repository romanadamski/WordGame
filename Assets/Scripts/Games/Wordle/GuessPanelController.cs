using System.Collections.Generic;
using UnityEngine;

public class GuessPanelController : MonoBehaviour
{
    [SerializeField]
    private EventChannelSO eventChannelSO;

    private GuessLineController[] lines;

    private void Awake()
    {
        lines = GetComponentsInChildren<GuessLineController>();
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
