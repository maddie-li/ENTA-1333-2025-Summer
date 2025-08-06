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

    /*public bool TargetInRange(Unit target, float range)
    {
        if (target == null || c == null)
        {
            return false;   
        }
        float distance = Vector3.Distance(transform.position, target.transform.position);
        ////Debug.Log(distance);

        return distance <= range;
    }*/

    public bool TargetInRange(Unit target, float range)
    {
        if (target == null || c == null)
            return false;

        Vector3 myPos = transform.position;

        List<Vector3> targetGridNodes = GridManager.Instance.GetOccupiedFootprintNodes(target);

        foreach (Vector3 nodePos in targetGridNodes)
        {
            float distance = Vector3.Distance(myPos, nodePos);
            if (distance <= range)
            {
                return true;
            }
        }

        return false;
    }

    public void Attack(Unit target)
    {
        if (target == null || Time.time - lastAttackTime < c.AttackCooldown) return;

        ////Debug.Log($"{name} attacks {target.name} {target.Army} for {c.Damage} damage.", target);
        FXManager.Instance.DoFX(FXType.CombatantAttack);
        target?.TakeDamage(c.Damage); 

        lastAttackTime = Time.time;
    }
}
