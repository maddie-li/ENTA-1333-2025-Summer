using UnityEngine;
using System.Collections.Generic;
using RTS_1333;
using System.Collections;

public class Combat : MonoBehaviour
{
    public Unit Unit;
    private UnitData UnitData;

    private float lastAttackTime;
    public bool IsAttacking;
    public bool IsDead;
    private Animator animator;
    private Movement movement;
    private Attack attack;
    private enum CombatantState { Idle, Chasing, Attacking }
    private CombatantState currentState = CombatantState.Idle;
    public Coroutine stateRoutine;

    private float sensingRange;
    private float attackRange;
    private Unit currentTarget;

    private void Awake()
    {
        Unit = GetComponentInParent<Unit>();
        UnitData = Unit.UnitData;

        if (Unit.TryGetComponent<Animator>(out Animator _animator))
            animator = _animator;

        if (Unit.TryGetComponent<Movement>(out Movement _movement))
            movement = _movement;

        if (Unit.TryGetComponent<Attack>(out Attack _attack))
            attack = _attack;
    }
    private void Start()
    {
        stateRoutine = StartCoroutine(StateMachine());

    }


    // BEHAVIOURS ---------------------------------------------------------------

    private IEnumerator StateMachine()
    {
        while (!IsDead)
        {
            switch (currentState)
            {
                case CombatantState.Idle:
                    yield return IdleBehavior();
                    break;
                case CombatantState.Chasing:
                    yield return ChaseBehaviour();
                    break;
                case CombatantState.Attacking:
                    yield return AttackBehavior();
                    break;
            }

            yield return null;
        }

    }

    private IEnumerator IdleBehavior()
    {
        UpdateAnimator();

        //Debug.LogWarning($"{name} entering Idle");
        // every 1 second check for enemy

        while (currentState == CombatantState.Idle)
        {
            Unit target = GetClosestEnemyUnit();

            if (target != null && attack.TargetInRange(target, sensingRange))
            {
                //Debug.Log($"{target.name} in sensing range");
                currentState = CombatantState.Chasing;
                yield break;
            }

            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator ChaseBehaviour()
    {
        UpdateAnimator();

        //Debug.LogWarning($"{name} entering Chase");
        // every 0.5 second try to go to target

        while (currentState == CombatantState.Chasing)
        {
            Unit target = GetClosestEnemyUnit();

            if (target == null)
            {
                currentState = CombatantState.Idle;
                yield break;
            }

            if (attack.TargetInRange(target, attackRange))
            {
                //Debug.Log($"{target.name} in attacking range");
                currentState = CombatantState.Attacking;
                yield break;
            }

            if (target.CurrentNode != null)
            {
                //Debug.Log($"In sensing range of {target.name} at {target.CurrentNode.Name}, begin chase");
                SetTarget(target.CurrentNode);
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator AttackBehavior()
    {
        UpdateAnimator();

        //Debug.LogWarning($"{name} entering Attack");
        // every 0.5 second do attack

        while (currentState == CombatantState.Attacking)
        {
            Unit target = GetClosestEnemyUnit();

            if (target == null)
            {
                currentState = CombatantState.Idle;
                yield break;
            }

            if (!attack.TargetInRange(target, attackRange))
            {
                currentState = CombatantState.Chasing;
                yield break;
            }

            this.transform.LookAt(target.transform);
            attack.AttackTarget(target);

            yield return new WaitForSeconds(0.5f);
        }
    }


    public void SetTarget(GridNode targetNode)
    {
        movement.SetTarget(targetNode);
    }

    // ENEMY MANAGEMENT ---------------------------------------------------------------

    private List<Unit> GetEnemies()
    {
        if (Unit.Army == Army.Enemy)
        {
            return UnitManager.Instance.UnitsByArmy[Army.Player];
        }
        else
        {
            return UnitManager.Instance.UnitsByArmy[Army.Enemy];
        }

    }
    private Unit GetClosestEnemyUnit()
    {
        Unit closest = null;
        float closestDist = Mathf.Infinity;
        Vector3 myPos = transform.position;

        foreach (Unit enemy in GetEnemies())
        {
            float dist = Vector3.Distance(myPos, enemy.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = enemy;
            }
        }

        return closest;
    }

    private void UpdateAnimator()
    {
        //Debug.Log($"Updating animator {animator.name}");

        animator.SetBool("isIdle", false);
        animator.SetBool("isMoving", false); 
        animator.SetBool("isAttacking", false);

        switch (currentState)
        {
            case CombatantState.Idle:
                animator.SetBool("isIdle", true);
                break;
            case CombatantState.Chasing:
                animator.SetBool("isMoving", true);
                break;
            case CombatantState.Attacking:
                animator.SetBool("isAttacking", true);
                break;
        }
    }

}
