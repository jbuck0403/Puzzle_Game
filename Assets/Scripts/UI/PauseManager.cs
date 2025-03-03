using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenuCanvas;

    private MenuManager menuManager;

    public BackgroundMusicManager bgmm;

    [SerializeField]
    private KeyCode pauseKey = KeyCode.Escape;

    [SerializeField]
    private PlayerInput playerInput;
    private bool isPaused = false;

    void Awake()
    {
        menuManager = GetComponent<MenuManager>();
    }

    void Start()
    {
        bgmm = FindFirstObjectByType<BackgroundMusicManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        pauseMenuCanvas.SetActive(isPaused);
        menuManager.ShowEscapeMenu();
        Time.timeScale = isPaused ? 0f : 1f;

        playerInput.SetPaused(isPaused);
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseMenuCanvas.SetActive(false);
        Time.timeScale = 1f;

        playerInput.SetPaused(false);
    }

    public void QuitToMainMenu()
    {
        // unpause game
        ResumeGame();

        // start playing background music for main menu
        bgmm.PlayMainMenuMusic();

        // load main menu
        SceneManager.LoadScene(0);
    }
}
