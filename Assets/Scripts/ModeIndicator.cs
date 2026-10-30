using UnityEngine;
using UnityEngine.UI;

public class ModeIndicator : MonoBehaviour
{
    [SerializeField] Image attackIcon, platformIcon, jumpIcon;
    [SerializeField] Color activeColor = Color.white;
    [SerializeField] Color inactiveColor = new Color(1f, 1f, 1f, 0.3f);
    [SerializeField] float activeScale = 1.25f;

    void Update()
    {
        Show(attackIcon, VoiceControlMode.Attack);
        Show(platformIcon, VoiceControlMode.MovePlatforms);
        Show(jumpIcon, VoiceControlMode.Jump);
    }

    // Current mode's icon is bright and larger, the others are dimmed
    void Show(Image icon, VoiceControlMode mode)
    {
        if (icon == null) return;

        bool active = VoiceControl.CurrentMode == mode;
        icon.color = active ? activeColor : inactiveColor;
        icon.transform.localScale = Vector3.one * (active ? activeScale : 1f);
    }
}
