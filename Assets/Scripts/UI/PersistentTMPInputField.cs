using TMPro;
using UnityEngine.EventSystems;

public class PersistentTMPInputField : TMP_InputField
{
    public override void OnSubmit(BaseEventData eventData)
    {
        onSubmit?.Invoke(text);
    }
}