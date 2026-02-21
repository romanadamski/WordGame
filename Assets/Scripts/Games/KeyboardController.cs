using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum PolishKeyCode
{
    None = 0,
    A1, //•
    C1, //∆
    E1, // 
    L1, //£
    O1, //”
    S1, //å
    N1, //—
    Z1, //Ø
    Z2 //è
}

[Serializable]
public class CustomKeyCode
{
    public KeyCode KeyCode;
    public PolishKeyCode PolishKeyCode;
}

public class KeyboardController : MonoBehaviour
{
    [SerializeField]
    private GameObject polishCharacters;

    [SerializeField]
    private EventChannelSO eventChannelSO;

    private KeyController[] keyControllers;

    private void Awake()
    {
        keyControllers = GetComponentsInChildren<KeyController>();
        eventChannelSO.OnGameplayClear.AddListener(Clear);
    }

    public void ShowKeyboard(LanguageType language)
    {
        gameObject.SetActive(true);
        polishCharacters.SetActive(language == LanguageType.PL);
    }

    public void HideKeyboard()
    {
        gameObject.SetActive(false);
    }

    public void SetColors(List<LetterData> lettersColors)
    {
        foreach (LetterData letter in lettersColors)
        {
            var key = keyControllers.FirstOrDefault(x => letter.Character == (char)x.KeyCode.KeyCode
            || letter.Character == ParseToPolish(x.KeyCode.PolishKeyCode));
            key.SetColor(letter.Color, letter.GuessType);
        }
    }

    private void Clear()
    {
        foreach (var key in keyControllers)
        {
            key.Clear();
        }
    }


    public static char ParseToPolish(PolishKeyCode polishKeyCode)
    {
        return polishKeyCode switch
        {
            PolishKeyCode.None => '\0',
            PolishKeyCode.A1 => 'π',
            PolishKeyCode.C1 => 'Ê',
            PolishKeyCode.E1 => 'Í',
            PolishKeyCode.L1 => '≥',
            PolishKeyCode.O1 => 'Û',
            PolishKeyCode.S1 => 'ú',
            PolishKeyCode.N1 => 'Ò',
            PolishKeyCode.Z1 => 'ø',
            PolishKeyCode.Z2 => 'ü',
            _ => '\0',
        };
    }
}
