using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneAfter : MonoBehaviour
{
    public string sceneName;
    public float delay = 68f;

    void Start()
    {
        // Call the LoadScene function after the specified delay
        Invoke("LoadScene", delay);
    }

    void LoadScene()
    {
        // Load the specified scene
        SceneManager.LoadScene(sceneName);
    }
}
