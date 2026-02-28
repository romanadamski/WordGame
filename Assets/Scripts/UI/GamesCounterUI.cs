using TMPro;
using UnityEngine;

public class GamesCounterUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI gamesCounterLabel;

    [SerializeField]
    private TextMeshProUGUI winRatioLabel;

    private const string GAMES_COUNTER = "GAMES: ";
    private const string WINS = "WINS: ";

    public void InitCounter(int gamesCount, float winRatio)
    {
        gamesCounterLabel.text = $"{GAMES_COUNTER} {gamesCount}";
        winRatioLabel.text = $"{WINS} {Mathf.RoundToInt(winRatio)}%";
    }
}
