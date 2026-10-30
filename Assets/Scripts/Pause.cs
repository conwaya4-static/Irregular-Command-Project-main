using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public GameObject PausePanel;
    public GameObject firstSelected;   // optional: defaults to the sensitivity slider
    public Slider sensitivitySlider;
    public Player player;

    public bool isPaused = false;

    void Start()
    {
        if (PausePanel != null)
        {
            PausePanel.SetActive(false);
        }

        if (sensitivitySlider != null && player != null)
        {
            sensitivitySlider.value = player.loudnessSensitivity;
            sensitivitySlider.onValueChanged.AddListener(v => player.loudnessSensitivity = v);
        }
    }

    void Update()
    {
        bool escape = Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame;
        bool options = Gamepad.current != null && Gamepad.current.startButton.wasPressedThisFrame;

        if (escape || options)
            TogglePause();
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        PausePanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;

        if (EventSystem.current == null) return;

        GameObject target = null;
        if (isPaused)
            target = firstSelected != null ? firstSelected : (sensitivitySlider != null ? sensitivitySlider.gameObject : null);

        // Select the first menu item so the controller can navigate right away
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(target);
    }
}
