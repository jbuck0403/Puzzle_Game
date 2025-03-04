using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneButton : TextButton
{
    [SerializeField]
    private string sceneToLoad;

    private BackgroundMusicManager bgmm;

    // private VolumeSettings volumeSettings;

    private void Start()
    {
        bgmm = FindObjectOfType<BackgroundMusicManager>();
        // volumeSettings = FindObjectOfType<VolumeSettings>();
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
