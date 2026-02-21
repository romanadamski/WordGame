using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameplayMenuDM : BaseMenuDM
{
    public string Title;
    public LanguageType Language;
    public UnityAction OnCloseClick;
}

public class GameplayMenu : BaseMenu<GameplayMenuDM>
{
    [SerializeField]
    private TextMeshProUGUI title;
    
    [SerializeField]
    private TextMeshProUGUI language;
    
    [SerializeField]
    private Button closeButton;

    public override void Show(GameplayMenuDM menuDM)
    {
        title.text = menuDM.Title;
        language.text = menuDM.Language.ToString();
        closeButton.onClick.AddListener(menuDM.OnCloseClick);

        base.Show(menuDM);
    }

    public override void Hide()
    {
        closeButton.onClick.RemoveAllListeners();

        base.Hide();
    }
}
