using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameEndComputer : MonoBehaviour, IInteractable
{
    [SerializeField]
    private float fadeOutDuration = 3f;

    [SerializeField]
    private Image fadeImage;

    [SerializeField]
    private TMP_Text congratsText;

    public string PromptText { get; private set; } = "Finish the Game"; // Optional custom prompt text

    [SerializeField]
    private float textFadeInDelay = 1f;

    [SerializeField]
    private float textFadeInDuration = 1f;

    public bool CanInteract { get; private set; } = true;
    public float InteractRange => InteractDistance.Short;

    private void Start()
    {
        // Create a black UI image for fading if not assigned
        if (fadeImage == null)
        {
            // Create a new Canvas
            GameObject canvasObj = new GameObject("FadeCanvas");
            Canvas canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 999; // Ensure it's on top

            // Add CanvasScaler
            CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);

            // Create fade image
            GameObject imageObj = new GameObject("FadeImage");
            imageObj.transform.SetParent(canvasObj.transform, false);
            fadeImage = imageObj.AddComponent<Image>();
            fadeImage.color = new Color(0, 0, 0, 0);
            fadeImage.rectTransform.anchorMin = Vector2.zero;
            fadeImage.rectTransform.anchorMax = Vector2.one;
            fadeImage.rectTransform.sizeDelta = Vector2.zero;

            // Create congratulations text
            GameObject textObj = new GameObject("CongratsText");
            textObj.transform.SetParent(canvasObj.transform, false);
            congratsText = textObj.AddComponent<TextMeshProUGUI>();
            congratsText.text = "CONGRATULATIONS!";
            congratsText.fontSize = 72;
            congratsText.alignment = TextAlignmentOptions.Center;
            congratsText.color = new Color(1, 1, 1, 0); // Start fully transparent

            // Center the text
            RectTransform textRect = congratsText.rectTransform;
            textRect.anchorMin = new Vector2(0.5f, 0.5f);
            textRect.anchorMax = new Vector2(0.5f, 0.5f);
            textRect.pivot = new Vector2(0.5f, 0.5f);
            textRect.sizeDelta = new Vector2(800, 100);
            textRect.anchoredPosition = Vector2.zero;
        }
    }

    public bool StartInteract(Transform interactor)
    {
        if (!CanInteract)
            return false;

        StartCoroutine(EndGameSequence());
        CanInteract = false;
        return true;
    }

    public void EndInteract() { }

    private IEnumerator EndGameSequence()
    {
        print("ENDING GAME");
        // Disable player movement and input
        PlayerInput playerInput = FindObjectOfType<PlayerInput>();
        if (playerInput != null)
        {
            playerInput.enabled = false;
        }

        // Start fading to black
        float elapsedTime = 0f;
        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeOutDuration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        // Ensure screen is fully black
        fadeImage.color = Color.black;

        // Wait before showing text
        yield return new WaitForSeconds(textFadeInDelay);

        // Fade in congratulations text
        elapsedTime = 0f;
        while (elapsedTime < textFadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / textFadeInDuration);
            congratsText.color = new Color(1, 1, 1, alpha);
            yield return null;
        }

        // Ensure text is fully visible
        congratsText.color = Color.white;

        // Wait a moment before quitting
        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene(0);
    }
}
