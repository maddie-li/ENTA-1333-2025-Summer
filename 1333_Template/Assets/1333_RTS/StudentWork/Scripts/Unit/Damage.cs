using System.Collections;
using System.Collections.Generic;
using RTS_1333;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Damage : MonoBehaviour
{
    public Unit Unit;
    private UnitData UnitData;

    private int currentHP; 
    private Animator animator;
    [SerializeField] private HealthBar healthBar;

    [Header("Damage Settings")]
    public int CurrentHP => currentHP;
    public int MaxHP => UnitData.MaxHP;
    public int Defense => UnitData.Defense;

    private void Awake()
    {
        Unit = GetComponentInParent<Unit>();
        UnitData = Unit.UnitData;

        currentHP = MaxHP;
        if (healthBar != null)
            healthBar.SetHealth(currentHP, MaxHP);

        if (Unit.TryGetComponent<Animator>(out Animator _animator))
            animator = _animator;
    }
    public void TakeDamage(int damage)
    {
        if (animator != null)
        {
            //Debug.Log("Animate take damage");
            animator.SetTrigger("hasBeenDamaged");
        }

        /*if (TryGetComponent<Combatant>(out Combatant combatant))
        {
            //Debug.Log("Got damaged, stopping coroutine");
            //combatant.StopCoroutine(combatant.stateRoutine);
            FXManager.Instance.DoFX(FXType.CombatantDamage);
        }
        if (TryGetComponent<Building>(out Building building))
        {

            FXManager.Instance.DoFX(FXType.BuildingDamage);
        }*/

        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, MaxHP);
        UpdateHealthBar();

        if (currentHP <= 0)
            Die();
    }

    public void Heal(int amount)
    {
        currentHP += amount;
        currentHP = Mathf.Clamp(currentHP, 0, MaxHP);
        UpdateHealthBar();
    }

    protected virtual void Die()
    {
        if (!TryGetComponent<Unit>(out Unit thisUnit)) return;

        /*if (thisUnit is Combatant combatant)
        {
            combatant.Die();
            Debug.Log("Deregistering combatant");
            UnitManager.Instance.UnregisterUnit(combatant);
            FXManager.Instance.DoFX(FXType.CombatantDie);
        }
        else if (thisUnit is Building building)
        {
            Debug.Log("Deregistering building");
            BuildingManager.Instance.UnregisterUnit(building);
            FXManager.Instance.DoFX(FXType.BuildingDestroy);
        }*/

        if (thisUnit.Army == Army.Enemy)
        {
            CurrencyManager.Instance.EarnGold(Army.Player, thisUnit.Cost);
        }
        else
        {
            CurrencyManager.Instance.EarnGold(Army.Enemy, thisUnit.Cost);
        }

        if (animator != null)
        {
            Destroy(gameObject, 2f);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
            healthBar.SetHealth(currentHP, MaxHP);
    }
}
