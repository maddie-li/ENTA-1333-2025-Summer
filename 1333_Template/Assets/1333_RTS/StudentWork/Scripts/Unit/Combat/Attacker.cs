using System.Collections;
using System.Collections.Generic;
using RTS_1333;
using UnityEngine;
using UnityEngine.InputSystem;

public class Attacker : MonoBehaviour
{
    private CombatantType c;
    private float lastAttackTime;

    public void Initialize(CombatantType _combatantType)
    {
        c = _combatantType;
    }

    public bool TargetInRange(Unit target, float range)
    {
        if (target == null || c == null)
        {
            return false;   
        }
        float distance = Vector3.Distance(transform.position, target.transform.position);
        //Debug.Log(distance);

        return distance <= range;
    }

    public void Attack(Unit target)
    {
        if (target == null || Time.time - lastAttackTime < c.AttackCooldown) return;

        Debug.Log($"{name} attacks {target.name} for {c.Damage} damage.");
        AudioManager.Instance.PlaySFX(SFX.DealDamage);
        target.TakeDamage(c.Damage); 

        lastAttackTime = Time.time;
    }
}
