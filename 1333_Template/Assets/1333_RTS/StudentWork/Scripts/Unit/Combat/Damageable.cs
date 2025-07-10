using RTS_1333;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField] private HealthBar healthBar;
    private Animator animator;

    private int maxHP;
    private int currentHP;

    public int CurrentHP => currentHP;
    public int MaxHP => maxHP;

    public void Initialize(int _maxHP, Animator _animator)
    {
        animator = _animator;
        Initialize(_maxHP);
    }

    public void Initialize(int _maxHP)
    {

        maxHP = _maxHP;

        currentHP = maxHP;
        if (healthBar != null)
            healthBar.SetHealth(currentHP, maxHP);
    }

    public void TakeDamage(int damage)
    {
        // visuals
        ParticleManager.Instance.PlayParticle(ParticleType.Blood, transform.position);
        if (animator != null)
        {
            Debug.Log("Animate take damage");
            animator.SetTrigger("hasBeenDamaged");
            AudioManager.Instance.PlaySFX(SFX.TakeDamage);
        }


        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        UpdateHealthBar();

        if (currentHP <= 0)
            Die();
    }

    public void Heal(int amount)
    {
        AudioManager.Instance.PlaySFX(SFX.Heal);
        currentHP += amount;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        UpdateHealthBar();
    }

    protected virtual void Die()
    {
        if (animator != null)
        {
            AudioManager.Instance.PlaySFX(SFX.Die);
            Debug.Log("Animate die");
            animator.SetTrigger("hasDied");
        }

        // uhoh youre died

        if (TryGetComponent<Combatant>(out Combatant combatant))
        {
            Debug.Log("Deregistering combatant");
            UnitManager.Instance.UnregisterUnit(combatant);

        }

        if (TryGetComponent<Building>(out Building building))
        {
            Debug.Log("Deregistering building");
            BuildingManager.Instance.UnregisterUnit(building);
        }
        Destroy(gameObject, 5f);
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
            healthBar.SetHealth(currentHP, maxHP);
    }
}
