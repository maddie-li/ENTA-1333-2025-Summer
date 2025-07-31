using System.Collections;
using System.Collections.Generic;
using RTS_1333;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Attack : MonoBehaviour
{
    public Unit Unit;
    private UnitData UnitData;

    private float lastAttackTime;
    private Animator animator;

    [Header("Attack Settings")]
    [SerializeField] private int damage = 1;
    [SerializeField] private int range = 1;
    [SerializeField] private float cooldown = 1f;

    public int Damage => damage;
    public int Range => range;
    public float Cooldown => cooldown;

    private void Awake()
    {
        Unit = GetComponentInParent<Unit>();
        UnitData = Unit.UnitData;

        if (Unit.TryGetComponent<Animator>(out Animator _animator))
            animator = _animator;
    }

    public bool TargetInRange(Unit target, float range)
    {
        if (target == null || UnitData == null)
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

    public void AttackTarget(Unit target)
    {
        if (target == null || Time.time - lastAttackTime < cooldown) return;

        Debug.Log($"{name} attacks {target.name} for {damage} damage.");
        FXManager.Instance.DoFX(FXType.CombatantAttack);
        target?.TakeDamage(damage); 

        lastAttackTime = Time.time;
    }
}
