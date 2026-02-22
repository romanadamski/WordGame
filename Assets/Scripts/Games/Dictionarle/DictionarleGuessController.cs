using TMPro;
using UnityEngine;

public class DictionarleGuessController : MonoBehaviour
{
    [field: SerializeField]
    public TextMeshProUGUI GuessLabel {  get; private set; }

    public string Text { get; set; }
}
