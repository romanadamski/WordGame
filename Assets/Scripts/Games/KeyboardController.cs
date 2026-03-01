using Solo.MOST_IN_ONE;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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
    private EventChannelSO eventChannelSO;

    [SerializeField]
    private GameObject polishCharacters;
    
    [SerializeField]
    private TextMeshProUGUI keyHighlight;

    private KeyController[] keyControllers;
    private Coroutine hightightCoroutine;

    private void Awake()
    {
        keyControllers = GetComponentsInChildren<KeyController>();
        eventChannelSO.OnKeyDown.AddListener(OnKeyDown);
        eventChannelSO.OnGameplayClear.AddListener(Clear);
        keyHighlight.transform.parent.gameObject.SetActive(false);
    }

    private void OnKeyDown(KeyController key)
    {
        ShowKeyHighlight(key);
        MOST_HapticFeedback.Generate(MOST_HapticFeedback.HapticTypes.MediumImpact);
    }

    private void ShowKeyHighlight(KeyController key)
    {
        keyHighlight.text = key.GetComponentInChildren<TextMeshProUGUI>().text;

        keyHighlight.transform.parent.position = key.transform.position + Vector3.up * 110f;
        (keyHighlight.transform.parent as RectTransform).sizeDelta = (key.transform as RectTransform).sizeDelta;

        if (hightightCoroutine != null)
        {
            StopCoroutine(hightightCoroutine);
        }
        hightightCoroutine = StartCoroutine(ShowKeyHighlightCoroutine());
    }

    private IEnumerator ShowKeyHighlightCoroutine()
    {
        keyHighlight.transform.parent.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        keyHighlight.transform.parent.gameObject.SetActive(false);
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
