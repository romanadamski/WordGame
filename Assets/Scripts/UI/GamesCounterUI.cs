using TMPro;
using UnityEngine;

public class GamesCounterUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI gamesCounterLabel;

    [SerializeField]
    private TextMeshProUGUI currentStreakLabel;
    
    [SerializeField]
    private TextMeshProUGUI winRatioLabel;

    private const string GAMES_COUNTER = "GAMES: ";
    private const string CURRENT_STREAK = "STREAK: ";
    private const string WINS = "WIN RATIO: ";

    public void InitCounter(int gamesCount, int streakCount, float winRatio)
    {
        gamesCounterLabel.text = $"{GAMES_COUNTER} {gamesCount}";
        currentStreakLabel.text = $"{CURRENT_STREAK} {streakCount}";
        winRatioLabel.text = $"{WINS} {Mathf.RoundToInt(winRatio)}%";
    }
}
