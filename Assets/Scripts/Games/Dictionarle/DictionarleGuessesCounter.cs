using System.Collections.Generic;
using UnityEngine;

public class DictionarleGuessesCounter : MonoBehaviour
{
    [SerializeField]
    private DictionarleGuessNumber guessNumberPrefab;
    
    [SerializeField]
    private Color inactiveGuessNumber;
    
    [SerializeField]
    private Color activeGuessNumber;
    
    [SerializeField]
    private Color missedGuessNumber;
    
    [SerializeField]
    private Color correctGuessNumber;

    private readonly List<DictionarleGuessNumber> guessNumbers = new();
    private int currentIndex;

    public void Init(int maxTries)
    {
        guessNumbers.ForEach(x => Destroy(x.gameObject));
        guessNumbers.Clear();

        for (int i = 0; i < maxTries; i++)
        {
            var guessNumber = Instantiate(guessNumberPrefab, transform);
            guessNumber.Background.color = inactiveGuessNumber;
            guessNumber.Label.gameObject.SetActive(false);
            guessNumbers.Add(guessNumber);
        }
        currentIndex = 0;
        guessNumbers[currentIndex].Background.color = activeGuessNumber;
        guessNumbers[currentIndex].Label.gameObject.SetActive(true);
        guessNumbers[currentIndex].Label.text = $"{currentIndex + 1}";
    }

    public void Win()
    {
        guessNumbers[currentIndex].Background.color = correctGuessNumber;
    }

    public void MoveToNextNumber()
    {
        guessNumbers[currentIndex].Background.color = missedGuessNumber;
        currentIndex++;
        if (currentIndex < guessNumbers.Count)
        {
            guessNumbers[currentIndex].Background.color = activeGuessNumber;
            guessNumbers[currentIndex].Label.gameObject.SetActive(true);
            guessNumbers[currentIndex].Label.text = $"{currentIndex + 1}";
        }
    }
}
