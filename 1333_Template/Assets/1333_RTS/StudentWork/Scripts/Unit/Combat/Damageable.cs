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
        if (animator != null)
        {
            Debug.Log("Animate take damage");
            animator.SetTrigger("hasBeenDamaged");
        }

        if (TryGetComponent<Combatant>(out Combatant combatant))
        {
            Debug.Log("Got damaged, stopping coroutine");
            //combatant.StopCoroutine(combatant.stateRoutine);
            FXManager.Instance.DoFX(FXType.CombatantDamage);
        }

        if (TryGetComponent<Building>(out Building building))
        {

            FXManager.Instance.DoFX(FXType.BuildingDamage);
        }

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

        if (TryGetComponent<Combatant>(out Combatant combatant))
        {
            combatant.Die();

            Debug.Log("Deregistering combatant");
            UnitManager.Instance.UnregisterUnit(combatant);

            FXManager.Instance.DoFX(FXType.CombatantDie);
        }

        if (TryGetComponent<Building>(out Building building))
        {
            Debug.Log("Deregistering building");
            BuildingManager.Instance.UnregisterUnit(building);

            FXManager.Instance.DoFX(FXType.BuildingDestroy);
            Destroy(gameObject);
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
            healthBar.SetHealth(currentHP, maxHP);
    }
}
