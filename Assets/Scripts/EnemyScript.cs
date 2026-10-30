using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyScript : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float moveSpeed = 2f, detectionRange = 8f, attackRange = 1.5f, attackCooldown = 1.5f;
    [SerializeField] float wallCheck = 0.15f, ledgeCheck = 1f, ledgeForward = 0.2f;
    [SerializeField] int damage = 1, health = 3;

    Animator anim;
    Rigidbody2D rb;
    Collider2D col;
    SpriteRenderer sr;
    float nextAttack, attackStart;
    bool attacking;
    int facing = 1;

    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        if (player == null) player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void FixedUpdate()
    {
        float vy = rb.linearVelocity.y;
        rb.linearVelocity = new Vector2(0, vy);

        // Failsafe: if the EndAttack event never fires, don't stay stuck forever
        if (attacking && Time.time > attackStart + 2f) attacking = false;

        if (player != null && !attacking)
            Chase(vy);

        anim.SetBool("Running", Mathf.Abs(rb.linearVelocity.x) > 0.01f);
    }

    void Chase(float vy)
    {
        float dist = Vector2.Distance(transform.position, player.position);
        int dir = player.position.x >= transform.position.x ? 1 : -1;
        if (dir != facing)
        {
            facing = dir;
            var s = transform.localScale;
            s.x = Mathf.Abs(s.x) * facing;
            transform.localScale = s;
        }

        if (dist <= attackRange)
        {
            if (Time.time >= nextAttack)
            {
                attacking = true;
                attackStart = Time.time;
                nextAttack = Time.time + attackCooldown;
                anim.SetTrigger("Attack");
            }
        }
        else if (dist <= detectionRange && CanMove())
            rb.linearVelocity = new Vector2(facing * moveSpeed, vy);
    }

    bool CanMove()
    {
        Bounds b = col.bounds;
        var wallPos = new Vector2(b.center.x + facing * (b.extents.x + 0.02f), b.center.y);
        if (Physics2D.Raycast(wallPos, Vector2.right * facing, wallCheck, groundLayer)) return false;

        var ledgePos = new Vector2(b.center.x + facing * (b.extents.x + ledgeForward), b.min.y + 0.05f);
        return Physics2D.Raycast(ledgePos, Vector2.down, ledgeCheck, groundLayer);
    }

    // Animation events on the Attack clip
    public void DealDamage()
    {
        if (player == null || Vector2.Distance(transform.position, player.position) > attackRange + 0.5f) return;

        Player p = player.GetComponentInParent<Player>();
        if (p != null) p.TakeDamage(damage);
    }

    public void EndAttack() => attacking = false;

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0) { Destroy(gameObject); return; }

        attacking = false;          // a hit interrupts the enemy's attack
        anim.SetTrigger("Hurt");

        if (sr != null) { sr.color = Color.red; Invoke(nameof(ResetColor), 0.1f); }
    }

    void ResetColor() { if (sr != null) sr.color = Color.white; }
}
