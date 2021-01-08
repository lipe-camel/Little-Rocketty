using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    public void LoadStartingScene()
    {
        SceneManager.LoadScene(0);
    }

}
