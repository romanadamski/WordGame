using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameStateSO", menuName = "ScriptableObjects/Game State")]
public class GameStateSO : ScriptableObject
{
    [NonSerialized]
    public LanguageConfig CurrentLanguageConfig;
}
