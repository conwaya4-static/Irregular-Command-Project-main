using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTrigger : MonoBehaviour
{
   [Tooltip("Build index of the scene to load (File → Build Settings).")]
    public int sceneIndex;

    void OnTriggerEnter2D(Collider2D other)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
