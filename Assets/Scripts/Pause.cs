using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public GameObject PausePanel;
    public GameObject firstSelected;   // optional: defaults to the sensitivity slider
    public Slider sensitivitySlider;
    public Slider volumeSlider;
    public Player player;

    public bool isPaused = false;

    void Awake()
    {
        if (volumeSlider != null)
        {
            // Smooth 0-1 range so volume can be anywhere between silent and full
            volumeSlider.wholeNumbers = false;
            volumeSlider.minValue = 0f;
            volumeSlider.maxValue = 1f;
            volumeSlider.value = AudioListener.volume;
            volumeSlider.onValueChanged.AddListener(v => AudioListener.volume = v);
        }
    }

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
