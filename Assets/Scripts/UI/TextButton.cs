using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class TextButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Color hoverColor = Color.black;
    private TMP_Text tmpText;
    private Color originalColor;
    private Button button;

    private UIButtonAudioHandler audioHandler;

    protected virtual void Awake()
    {
        button = gameObject.GetComponent<Button>();
        if (button == null)
        {
            button = gameObject.AddComponent<Button>();
        }

        tmpText = GetComponent<TMP_Text>();
        originalColor = tmpText.color;

        button.onClick.AddListener(HandleButtonPress);

        audioHandler = GetComponent<UIButtonAudioHandler>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (audioHandler != null)
        {
            print("hover sound");
            audioHandler.PlayHoverSound();
        }
        tmpText.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tmpText.color = originalColor;
    }

    private void HandleButtonPress()
    {
        if (audioHandler != null)
        {
            audioHandler.PlayClickSound();
        }
        OnButtonPress();
    }

    protected abstract void OnButtonPress();
}
