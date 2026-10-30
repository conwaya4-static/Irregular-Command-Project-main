using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FlyingEnemy : MonoBehaviour, IDamageable
{
    [SerializeField] Transform player, firePoint;
    [SerializeField] EnemyProjectile projectilePrefab;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float moveSpeed = 3f, detectionRange = 12f, shootRange = 6f, closeRange = 1.2f, shootCooldown = 2f;
    [SerializeField] float bobSpeed = 3f, bobAmount = 0.8f, deathDelay = 1f;
    [SerializeField] int health = 3;

    Animator anim;
    Rigidbody2D rb;
    Collider2D col;
    SpriteRenderer sr;
    float nextShot, shootStart;
    bool shooting, dead;
    int facing = 1;

    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
        if (player == null) player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void FixedUpdate()
    {
        if (dead) return;

        // Default: hover in place, bobbing gently up and down
        Vector2 vel = new Vector2(0f, Mathf.Sin(Time.time * bobSpeed) * bobAmount);
        bool moving = false;

        // Failsafe: if the EndShoot event never fires, don't stay stuck forever
        if (shooting && Time.time > shootStart + 2f) shooting = false;

        if (player != null)
        {
            Vector2 toPlayer = player.position - transform.position;
            float dist = toPlayer.magnitude;
            Face(toPlayer.x >= 0 ? 1 : -1);

            if (!shooting && dist <= detectionRange)
            {
                bool canSee = !Physics2D.Linecast(transform.position, player.position, groundLayer);

                // Distance from the player to the nearest edge of this enemy's collider.
                // Staying within closeRange keeps the bot inside the player's melee reach.
                float edgeDist = Vector2.Distance(col.ClosestPoint(player.position), player.position);

                if (canSee && dist <= shootRange && Time.time >= nextShot)
                {
                    shooting = true;
                    shootStart = Time.time;
                    nextShot = Time.time + shootCooldown;
                    anim.SetTrigger("Shoot");
                }
                else if (edgeDist > closeRange || !canSee)
                {
                    vel = toPlayer.normalized * moveSpeed;   // keep flying toward the player
                    moving = true;
                }
            }
        }

        rb.linearVelocity = vel;
        anim.SetBool("Moving", moving);
    }

    void Face(int dir)
    {
        if (dir == facing) return;
        facing = dir;
        var s = transform.localScale;
        s.x = Mathf.Abs(s.x) * facing;
        transform.localScale = s;
    }

    // Animation events on the shoot clip
    public void Fire()
    {
        if (dead || player == null || projectilePrefab == null) return;

        Vector2 origin = firePoint != null ? (Vector2)firePoint.position : (Vector2)transform.position;
        Vector2 dir = ((Vector2)player.position - origin).normalized;
        Instantiate(projectilePrefab, origin, Quaternion.identity).Launch(dir, transform);
    }

    public void EndShoot() => shooting = false;

    public void TakeDamage(int amount)
    {
        if (dead) return;

        health -= amount;
        if (health <= 0)
        {
            dead = true;
            rb.linearVelocity = Vector2.zero;
            col.enabled = false;
            anim.SetTrigger("Death");
            Destroy(gameObject, deathDelay);
            return;
        }

        if (sr != null) { sr.color = Color.red; Invoke(nameof(ResetColor), 0.1f); }
    }

    void ResetColor() { if (sr != null) sr.color = Color.white; }
}
