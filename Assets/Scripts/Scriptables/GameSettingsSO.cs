using System;
using UnityEngine;

public enum LanguageType
{
    PL,
    EN
}

[Serializable]
public class LanguageConfig
{
    [field: SerializeField]
    public LanguageType Language { get; private set; }

    [field: SerializeField]
    public TextAsset Answers { get; private set; }

    [field: SerializeField]
    public TextAsset Guesses { get; private set; }
}

[CreateAssetMenu(fileName = "GameSettingsSO", menuName = "ScriptableObjects/Game Settings")]
public class GameSettingsSO : ScriptableObject
{
    [field: SerializeField]
    public LanguageConfig PolishLanguage {  get; private set; }

    [field: SerializeField]
    public LanguageConfig EnglishLanguage {  get; private set; }

    [field: SerializeField]
    public LanguageType DefaultLanguage { get; private set; }

    [field: SerializeField]
    public Color GoodColor1 { get; private set; }

    [field: SerializeField]
    public Color GoodColor2 { get; private set; }

    [field: SerializeField]
    public Color WrongColor { get; private set; }
}
