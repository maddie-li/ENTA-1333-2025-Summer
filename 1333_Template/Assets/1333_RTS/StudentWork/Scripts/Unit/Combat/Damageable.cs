using UnityEngine;

public class Damageable : MonoBehaviour
{
    private int maxHP;
    private int currentHP;

    [SerializeField] private HealthBar healthBar;

    public int CurrentHP => currentHP;
    public int MaxHP => maxHP;

    public void Initialize(int maxHealth)
    {
        maxHP = maxHealth;
        currentHP = maxHP;
        if (healthBar != null)
            healthBar.SetHealth(currentHP, maxHP);
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        UpdateHealthBar();

        if (currentHP <= 0)
            Die();
    }

    public void Heal(int amount)
    {
        currentHP += amount;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        UpdateHealthBar();
    }

    protected virtual void Die()
    {
        // uhoh youre died
        gameObject.SetActive(false);
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
            healthBar.SetHealth(currentHP, maxHP);
    }
}
