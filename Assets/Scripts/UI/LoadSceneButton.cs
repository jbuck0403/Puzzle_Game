using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneButton : TextButton
{
    [SerializeField]
    private string sceneToLoad;

    private BackgroundMusicManager bgmm;

    private void Start()
    {
        bgmm = FindFirstObjectByType<BackgroundMusicManager>();
    }

    protected override void OnButtonPress()
    {
        LoadScene();
    }

    private void LoadScene()
    {
        if (bgmm != null)
        {
            bgmm.PlayLevelMusic(sceneToLoad);
        }
        SceneManager.LoadScene(sceneToLoad);
    }
}
