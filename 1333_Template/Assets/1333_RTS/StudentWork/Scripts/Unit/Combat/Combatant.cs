using UnityEngine;
using System.Collections.Generic;
using RTS_1333;
using System.Collections;

public class Combatant : Unit, ISelectableObject
{
    public bool IsAttacking;
    public bool IsDead;

    //[Header("References")]
    private MovementController movement;
    private Animator animator;
    private Attacker atk;

    //[Header("Visuals")]
    private Renderer[] renderers;
    private Material defaultMat;
    private Material selectedMat;

    //[Header("State Machine")]
    private enum CombatantState { Idle, Chasing, Attacking }
    private CombatantState currentState = CombatantState.Idle;
    public Coroutine stateRoutine;

    private float sensingRange;
    private float attackRange;
    private Combatant currentTarget;

    // SETUP  ---------------------------------------------------------------

    private void Awake()
    {
        movement = GetComponent<MovementController>();
        animator = GetComponentInChildren<Animator>();
        atk = GetComponent<Attacker>();

        InitDamage();
    }
    private void Start()
    {
        stateRoutine = StartCoroutine(StateMachine());

        /*if (this.army == Army.Player)
        {
            stateRoutine = StartCoroutine(StateMachine());
        }*/

    }

    public void Initialize(Pathfinder _pathfinder)
    {
        if (movement == null || atk == null)
        {
            //Debug.LogError("Combatant missing movement and attack components");
            return;
        }

        float moveSpeed = 3f; // set default

        if (unitType is CombatantType combatantType)
        {
            moveSpeed = combatantType.MoveSpeed;
            sensingRange = combatantType.SensingRange;
            attackRange = combatantType.AttackRange;

            atk.Initialize(combatantType);
        }
        else
        {
            //Debug.LogWarning("Wrong unit type on Combatant");
        }

            movement.Initialize(_pathfinder, this, moveSpeed);
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
            Unit target = GetClosestTarget();

            if (target != null && atk.TargetInRange(target, sensingRange))
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
            Unit target = GetClosestTarget();

            if (target == null)
            {
                currentState = CombatantState.Idle;
                yield break;
            }

            if (atk.TargetInRange(target, attackRange))
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
            Unit target = GetClosestTarget();

            if (target == null)
            {
                currentState = CombatantState.Idle;
                yield break;
            }

            if (!atk.TargetInRange(target, attackRange))
            {
                currentState = CombatantState.Chasing;
                yield break;
            }

            this.transform.LookAt(target.transform);
            atk.Attack(target);

            yield return new WaitForSeconds(0.5f);
        }
    }
    private IEnumerator DeadBehaviour()
    {
        yield break;
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

    private Building GetClosestEnemyBuilding()
    {
        Building closestBuilding = null;
        float closestDistance = Mathf.Infinity;
        Vector3 myPos = transform.position;

        foreach (Building building in GetEnemyBuildings())
        {
            float distance = Vector3.Distance(myPos, building.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestBuilding = building;
            }
        }

        return closestBuilding;
    }

    private Unit GetClosestTarget()
    {
        Vector3 myPosition = transform.position;

        Building closestBuilding = GetClosestEnemyBuilding();
        Combatant closestEnemy = GetClosestEnemy();

        float buildingDistance = closestBuilding != null
            ? Vector3.Distance(myPosition, closestBuilding.transform.position)
            : float.MaxValue;

        float enemyDistance = closestEnemy != null
            ? Vector3.Distance(myPosition, closestEnemy.transform.position)
            : float.MaxValue;

        if (buildingDistance < enemyDistance && closestBuilding != null)
            return closestBuilding;

        if (closestEnemy != null)
            return closestEnemy;

        return null;
    }


    private List<Building> GetEnemyBuildings()
    {
        if (army == Army.Enemy)
        {
            return BuildingManager.Instance.buildingsByArmy[Army.Player];
        }
        else
        {
            return BuildingManager.Instance.buildingsByArmy[Army.Enemy];
        }


    }

    // VISUALISATION ---------------------------------------------------------------
    public void SetupMat(Material regular, Material selected)
    {
        //Debug.Log("Setting up materials");
        renderers = GetComponentsInChildren<Renderer>();
        //Debug.Log(renderers.Length);
        //defaultMat = GetComponentInChildren<Renderer>().material;
        defaultMat = regular;
        selectedMat = selected;

        if (renderers == null) Debug.Log("Renderers are null");

        foreach (var rend in renderers)
        {
            if (rend.material != null)
                rend.material = defaultMat;
        }
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

    public void Die()
    {
        if (IsDead) return;

        //Debug.LogWarning($"{name} has died.");

        IsDead = true;

        if(army == Army.Enemy)
        {
            CurrencyManager.Instance.EarnGold(Army.Player, Cost);
        }
        else
        {
            CurrencyManager.Instance.EarnGold(Army.Enemy, Cost);
        }


            StopAllCoroutines();

        animator.SetBool("isIdle", false);
        animator.SetBool("isMoving", false);
        animator.SetBool("isAttacking", false);
        animator.SetBool("hasDied", true);

        /*// stop other states
        currentState = CombatantState.Dead;
        UpdateAnimator();*/

        // disable action components
        if (movement != null) movement.enabled = false;
        if (atk != null) atk.enabled = false; 
        
        Destroy(gameObject, 2f);
    }
}
