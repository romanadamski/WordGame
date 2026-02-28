using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "EventChannelSO", menuName = "ScriptableObjects/Event Channel")]
public class EventChannelSO : ScriptableObject
{
    [NonSerialized]
    public UnityEvent<CustomKeyCode> OnKeyClick = new();

    [NonSerialized]
    public UnityEvent OnGameplayClear = new();

    [NonSerialized]
    public UnityEvent<bool, string> OnGameEnd = new();

    //todo state to unload - win/lose(gameend), go to mm
    [NonSerialized]
    public UnityEvent OnGameUnload = new();
}
