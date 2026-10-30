// Anything the player's attack can damage (ground enemy, flying enemy, ...)
public interface IDamageable
{
    void TakeDamage(int amount);
}
