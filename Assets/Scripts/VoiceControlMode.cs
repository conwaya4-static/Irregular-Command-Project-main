public enum VoiceControlMode
{
    None,
    Attack,
    MovePlatforms,
    Jump
}

/// <summary>
/// Shared voice-control mode set by gamepad face buttons on the player.
/// </summary>
public static class VoiceControl
{
    public static VoiceControlMode CurrentMode { get; set; } = VoiceControlMode.None;
}
