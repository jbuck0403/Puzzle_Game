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

    private void Awake()
    {
        button = gameObject.GetComponent<Button>();
        if (button == null)
        {
            button = gameObject.AddComponent<Button>();
        }

        tmpText = GetComponent<TMP_Text>();
        originalColor = tmpText.color;

        button.onClick.AddListener(OnButtonPress);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tmpText.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tmpText.color = originalColor;
    }

    protected abstract void OnButtonPress();
}
