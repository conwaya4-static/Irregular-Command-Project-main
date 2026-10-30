using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    [SerializeField] float      m_speed = 4.0f;
    [SerializeField] float      m_jumpForce = 7.5f;
    [SerializeField] int        maxHealth = 10;
    [SerializeField] Slider     healthSlider;
    [SerializeField] int        attackDamage = 1;
    [SerializeField] float      attackRange = 1.6f;
    [SerializeField] float      attackHitDelay = 0.15f, attackCooldown = 0.3f;
    [SerializeField] float      respawnDelay = 2f;

    private int                 health;
    private Animator            m_animator;
    private Rigidbody2D         m_body2d;
    private Sensor_Bandit       m_groundSensor;
    private bool                m_grounded = false;
    private bool                m_combatIdle = false;
    private bool                m_isDead = false;
    private float               m_nextAttack;
    private Vector3             m_spawnPoint;

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
        health = maxHealth;
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
        m_spawnPoint = transform.position;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = health;
            healthSlider.interactable = false;   // display only: can't be selected or dragged
        }
    }

	void Update () {
        // Don't react to the mic or input while the game is paused
        if (Time.timeScale == 0f)
            return;

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

        // Death / Recover (fires once when health crosses zero)
        if (health <= 0 && !m_isDead) {
            m_isDead = true;
            m_animator.SetTrigger("Death");
            StartCoroutine(Respawn());
        }
        else if (health > 0 && m_isDead) {
            m_isDead = false;
            m_animator.SetTrigger("Recover");
        }

        if (m_isDead) {
            m_body2d.linearVelocity = new Vector2(0f, m_body2d.linearVelocity.y);
            return;
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
        // Attack: starts the moment a sound begins and plays out fully, even if the sound stops
        if (VoiceControl.CurrentMode == VoiceControlMode.Attack && voiceJustActivated && Time.time >= m_nextAttack) {
            m_nextAttack = Time.time + attackCooldown;
            m_animator.SetTrigger("Attack");
            StartCoroutine(AttackHit());
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

    // Damages every enemy within attackRange of the player shortly after the attack starts
    IEnumerator AttackHit()
    {
        yield return new WaitForSeconds(attackHitDelay);
        if (m_isDead) yield break;

        var hit = new HashSet<IDamageable>();
        foreach (Collider2D c in Physics2D.OverlapCircleAll(transform.position, attackRange))
        {
            // Hitting an enemy projectile sends it back at whoever fired it
            EnemyProjectile bullet = c.GetComponent<EnemyProjectile>();
            if (bullet != null) { bullet.Deflect(); continue; }

            IDamageable enemy = c.GetComponentInParent<IDamageable>();
            if (enemy != null && hit.Add(enemy))
                enemy.TakeDamage(attackDamage);
        }

        Debug.Log(hit.Count > 0 ? "Player hit " + hit.Count + " enemy(s)" : "Player attack missed - no enemy in range");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    // After the death animation, send the player back to the start with full health
    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnDelay);

        transform.position = m_spawnPoint;
        m_body2d.linearVelocity = Vector2.zero;
        health = maxHealth;
        if (healthSlider != null)
            healthSlider.value = health;
        // Update() sees health > 0 while dead and plays the Recover animation
    }

    // Called by the enemy when its attack lands
    public void TakeDamage(int amount)
    {
        if (m_isDead) return;

        health = Mathf.Max(health - amount, 0);
        if (healthSlider != null)
            healthSlider.value = health;

        if (health > 0)
            m_animator.SetTrigger("Hurt");
    }

    void OnMove(InputAction.CallbackContext context)
    {
        // Left stick vertical: up = move left, down = move right
        Vector2 stick = context.ReadValue<Vector2>();
        m_moveInputX = stick.y;
        Debug.Log("Moving");
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
