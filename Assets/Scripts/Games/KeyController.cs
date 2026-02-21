using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class KeyController : MonoBehaviour
{
    [SerializeField]
    private EventChannelSO eventChannelSO;
    
    [SerializeField]
    private CustomKeyCode keyCode;
    public CustomKeyCode KeyCode => keyCode;
    
    private Image background;
    private Button button;
    private Color defaultColor;
    private GuessType guessType = GuessType.None;

    private void Awake()
    {
        button = GetComponent<Button>();
        background = GetComponent<Image>();
        button.onClick.AddListener(OnKeyClick);
        defaultColor = background.color;
    }

    private void OnKeyClick()
    {
        eventChannelSO.OnKeyClick?.Invoke(keyCode);
    }

    public void SetColor(Color color, GuessType guessType)
    {
        if ((int)guessType > (int)this.guessType)
        {
            background.color = color;
            this.guessType = guessType;
        }
    }

    public void Clear()
    {
        background.color = defaultColor;
        guessType = GuessType.None;
    }
}
