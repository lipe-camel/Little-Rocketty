using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    [SerializeField] float loadLevelDelay = 1f;


    public void LoadStartingScene()
    {
        SceneManager.LoadScene(0);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadNextSceneWithDelay()
    {
        Invoke("LoadNextScene", loadLevelDelay);
    }

    public void RestartSceneWithDelay()
    {
        Invoke("RestartScene", loadLevelDelay);
    }
}
