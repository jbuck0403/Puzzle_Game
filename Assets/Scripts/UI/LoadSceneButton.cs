using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneButton : TextButton
{
    [SerializeField]
    private string sceneToLoad;

    protected override void OnButtonPress()
    {
        LoadScene();
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
