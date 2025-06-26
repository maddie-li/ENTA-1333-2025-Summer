using System.Collections;
using System.Collections.Generic;
using RTS_1333;
using UnityEngine;

public class Attacker : MonoBehaviour
{
    private CombatantType combatantType;
    private float lastAttackTime;

    public float AttackRange => combatantType.Range;
    public float AttackCooldown => combatantType.AttackCooldown;

    public void Initialize(CombatantType _combatantType)
    {
        combatantType = _combatantType;
    }

    public bool CanAttack(Combatant target)
    {
        if (target == null) return false;
        float distance = Vector3.Distance(transform.position, target.transform.position);
        return distance <= AttackRange;
    }

    public void Attack(Combatant target)
    {
        if (target == null || Time.time - lastAttackTime < AttackCooldown) return;

        Debug.Log($"{name} attacks {target.name} for {combatantType.Damage} damage.");
        //target.TakeDamage(combatantType.Damage); 

        lastAttackTime = Time.time;
    }
}
