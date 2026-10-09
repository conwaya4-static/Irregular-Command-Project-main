using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour {

    [SerializeField] float      m_speed = 4.0f;
    [SerializeField] float      m_jumpForce = 7.5f;
    [SerializeField] int        health = 5;

    private Animator            m_animator;
    private Rigidbody2D         m_body2d;
    private Sensor_Bandit       m_groundSensor;
    private bool                m_grounded = false;
    private bool                m_combatIdle = false;
    private bool                m_isDead = false;

    PlayerControls controls;
    float m_moveInputX;

    // VoiceDetection
    public VoiceDetection detector;

    public float loudnessSensitivity = 50;
    public float threshold = 0.2f;

    private bool m_voiceWasAboveThreshold;

    void Awake()
    {
        controls = new PlayerControls();
    }

    void OnEnable()
    {
        controls.Gameplay.Enable();
        controls.Gameplay.Move.performed += OnMove;
        controls.Gameplay.Move.canceled += OnMove;
    }

    void OnDisable()
    {
        controls.Gameplay.Move.performed -= OnMove;
        controls.Gameplay.Move.canceled -= OnMove;
        controls.Gameplay.Disable();
    }

    void OnDestroy()
    {
        controls?.Dispose();
    }

    void Start () {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Bandit>();
    }

	void Update () {
        float loudness = 0f;
        if (detector != null)
            loudness = detector.GetLoudnessFromMicrophone() * loudnessSensitivity;

        bool voiceActive = loudness >= threshold;
        bool voiceJustActivated = voiceActive && !m_voiceWasAboveThreshold;
        m_voiceWasAboveThreshold = voiceActive;

        HandleModeSwitch();

        // Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State()) {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }

        // Check if character just started falling
        if(m_grounded && !m_groundSensor.State()) {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        // Stick up → left, stick down → right
        float inputX = m_moveInputX;

        // Swap direction of sprite depending on walk direction
        if (inputX > 0)
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        else if (inputX < 0)
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        // Move
        m_body2d.linearVelocity = new Vector2(inputX * m_speed, m_body2d.linearVelocity.y);

        // Set AirSpeed in animator
        m_animator.SetFloat("AirSpeed", m_body2d.linearVelocity.y);

        // -- Handle Animations --
        // Death
        if (health == 0) {
            if(!m_isDead)
                m_animator.SetTrigger("Death");
            else
                m_animator.SetTrigger("Recover");

            m_isDead = !m_isDead;
        }
        // Attack (voice, only in Attack mode)
        else if (VoiceControl.CurrentMode == VoiceControlMode.Attack && voiceActive) {
            m_animator.SetTrigger("Attack");
        }
        // Jump (voice, only in Jump mode)
        else if (VoiceControl.CurrentMode == VoiceControlMode.Jump && voiceJustActivated && m_grounded) {
            m_animator.SetTrigger("Jump");
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
            m_body2d.linearVelocity = new Vector2(m_body2d.linearVelocity.x, m_jumpForce);
            m_groundSensor.Disable(0.2f);
        }
        // Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
            m_animator.SetInteger("AnimState", 2);
        // Combat Idle
        else if (m_combatIdle)
            m_animator.SetInteger("AnimState", 1);
        // Idle
        else
            m_animator.SetInteger("AnimState", 0);
    }

    void OnMove(InputAction.CallbackContext context)
    {
        // Left stick vertical: up = move left, down = move right
        Vector2 stick = context.ReadValue<Vector2>();
        m_moveInputX = stick.y;
    }

    void HandleModeSwitch()
    {
        Gamepad pad = Gamepad.current;
        if (pad == null)
            return;

        if (pad.buttonSouth.wasPressedThisFrame)
            VoiceControl.CurrentMode = VoiceControlMode.Attack;
        else if (pad.buttonNorth.wasPressedThisFrame)
            VoiceControl.CurrentMode = VoiceControlMode.MovePlatforms;
        else if (pad.buttonEast.wasPressedThisFrame)
            VoiceControl.CurrentMode = VoiceControlMode.Jump;
    }
}
