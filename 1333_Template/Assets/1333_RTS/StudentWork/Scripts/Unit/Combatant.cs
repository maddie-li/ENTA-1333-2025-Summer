using UnityEngine;
using System.Collections.Generic;
using RTS_1333;
using System.Collections;
using static UnityEngine.GraphicsBuffer;
using NUnit.Framework;

public class Combatant : Unit, ISelectableObject
{
    public bool IsAttacking;

    //[Header("References")]
    private MovementController movement;
    private Attacker attack;

    //[Header("Visuals")]
    private Renderer[] renderers;
    private Material defaultMat;
    private Material selectedMat;

    //[Header("State Machine")]
    private enum CombatantState { Idle, Chasing, Attacking }
    private CombatantState currentState = CombatantState.Idle;
    private Coroutine stateRoutine;

    private float sensingRange;
    private float attackRange;
    private Combatant currentTarget;

    // SETUP  ---------------------------------------------------------------

    private void Awake()
    {
        movement = GetComponent<MovementController>();
        attack = GetComponent<Attacker>();
    }
    private void Start()
    {
        if(this.army == Army.Player)
        {
            stateRoutine = StartCoroutine(StateMachine());
        }

    }

    private void Update()
    {
    }

    public void Initialize(Pathfinder _pathfinder)
    {
        if (movement == null || attack == null)
        {
            Debug.LogError("Combatant missing movement and attack components");
            return;
        }

        float moveSpeed = 3f; // set default

        if (unitType is CombatantType combatantType)
        {
            moveSpeed = combatantType.MoveSpeed;
            sensingRange = combatantType.SensingRange;
            attackRange = combatantType.AttackRange;

            attack.Initialize(combatantType);
        }
        else
        {
            Debug.LogWarning("Wrong unit type on Combatant");
        }

            movement.Initialize(_pathfinder, this, moveSpeed);
    }


    // BEHAVIOURS ---------------------------------------------------------------

    private IEnumerator StateMachine()
    {
        while (true)
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
        Debug.LogWarning($"{name} entering Idle");
        // every 1 second check for enemy

        while (currentState == CombatantState.Idle)
        {
            Combatant target = GetClosestEnemy();

            if (target != null && attack.TargetInRange(target, sensingRange))
            {
                Debug.Log($"{target.name} in sensing range");
                currentState = CombatantState.Chasing;
                yield break;
            }

            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator ChaseBehaviour()
    {
        Debug.LogWarning($"{name} entering Chase");
        // every 0.5 second try to go to target

        while (currentState == CombatantState.Chasing)
        {
            Combatant target = GetClosestEnemy();

            if (target == null)
            {
                currentState = CombatantState.Idle;
                yield break;
            }

            if (attack.TargetInRange(target, attackRange))
            {
                Debug.Log($"{target.name} in attacking range");
                currentState = CombatantState.Attacking;
                yield break;
            }

            if (target.CurrentNode != null)
            {
                Debug.Log($"In sensing range of {target.name} at {target.CurrentNode.Name}");
                //SetTarget(target.CurrentNode);
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator AttackBehavior()
    {
        Debug.LogWarning($"{name} entering Attack");
        // every 0.5 second do attack

        while (currentState == CombatantState.Attacking)
        {
            Combatant target = GetClosestEnemy();

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

            attack.Attack(target);

            yield return new WaitForSeconds(0.5f);
        }
    }
   

    public void SetTarget(GridNode targetNode)
    {
        movement.SetTarget(targetNode);
    }

    // ENEMY MANAGEMENT ---------------------------------------------------------------

    private Combatant GetClosestEnemy()
    {
        Combatant closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        Vector3 myPos = transform.position;

        foreach (Combatant enemy in GetEnemies())
        {
            float distance = Vector3.Distance(myPos, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }

    private List<Combatant> GetEnemies()
    {
        if(army == Army.Enemy)
        {
            return UnitManager.Instance.unitsByArmy[Army.Player];
        }
        else
        {
            return UnitManager.Instance.unitsByArmy[Army.Enemy];
        }

            
    }

    // VISUALISATION ---------------------------------------------------------------
    public void SetupMat(Material selected)
    {
        //Debug.Log("Setting up materials");
        renderers = GetComponentsInChildren<Renderer>();
        //Debug.Log(renderers.Length);
        defaultMat = GetComponentInChildren<Renderer>().material;
        selectedMat = selected;
    }

    public void SetSelected(bool selected)
    {
        Debug.Log("Updating unit material");
        if (renderers == null) Debug.Log("Renderers are null");

        Material mat = selected ? selectedMat : defaultMat;

        foreach (var rend in renderers)
        {
            if (rend.material != null)
                rend.material = mat;
        }
    }
}
