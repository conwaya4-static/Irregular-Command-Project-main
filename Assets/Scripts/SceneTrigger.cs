using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTrigger : MonoBehaviour
{
    public int sceneIndex;

    void OnTriggerEnter2D(Collider2D other)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
