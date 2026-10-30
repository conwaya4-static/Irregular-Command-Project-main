using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] float speed = 8f, lifetime = 4f;
    [SerializeField] int damage = 1, reflectedDamage = 2;
    [SerializeField] LayerMask groundLayer;   // walls and floors that stop the projectile
    [SerializeField] Color reflectedColor = Color.cyan;

    Rigidbody2D rb;
    Transform owner;        // whoever fired it, so a deflected shot can be sent back
    bool reflected;

    public void Launch(Vector2 dir, Transform shooter)
    {
        owner = shooter;
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0f;
        GetComponent<Collider2D>().isTrigger = true;

        SetDirection(dir);
        Destroy(gameObject, lifetime);
    }

    // Called by the player's attack: sends the shot back at whoever fired it
    public void Deflect()
    {
        if (reflected) return;
        reflected = true;

        Vector2 dir = owner != null
            ? ((Vector2)owner.position - (Vector2)transform.position).normalized
            : -rb.linearVelocity.normalized;
        SetDirection(dir);

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.color = reflectedColor;
    }

    void SetDirection(Vector2 dir)
    {
        rb.linearVelocity = dir * speed;
        transform.right = dir;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (reflected)
        {
            // A deflected shot hurts enemies, never the player
            IDamageable target = other.GetComponentInParent<IDamageable>();
            if (target != null)
            {
                target.TakeDamage(reflectedDamage);
                Destroy(gameObject);
                return;
            }
        }
        else
        {
            Player p = other.GetComponentInParent<Player>();
            if (p != null)
            {
                p.TakeDamage(damage);
                Destroy(gameObject);
                return;
            }
        }

        if (((1 << other.gameObject.layer) & groundLayer) != 0)
            Destroy(gameObject);
    }
}
