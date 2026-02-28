using TMPro;
using UnityEngine;

public class DictionarleGuessLabel : MonoBehaviour
{
    [field: SerializeField]
    public TextMeshProUGUI GuessLabel {  get; private set; }

    public string Text { get; set; }

    public override string ToString()
    {
        return Text;
    }
}
