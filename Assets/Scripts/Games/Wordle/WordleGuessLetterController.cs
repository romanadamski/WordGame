using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WordleGuessLetterController : MonoBehaviour
{
    [field: SerializeField]
    public TextMeshProUGUI Letter {  get; private set; }

    [field: SerializeField]
    public Image Background { get; private set; }
}
