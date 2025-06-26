using UnityEngine;
using System.Collections.Generic;
using RTS_1333;

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

    private void Awake()
    {
        movement = GetComponent<MovementController>();
        attack = GetComponent<Attacker>();
    }

    private void Update()
    {
        if (IsAttacking)
        {
            Combatant enemy = GetClosestEnemy();
            if (enemy != null && enemy.CurrentNode != null)
            {
                Debug.Log($"Attacking {enemy} at {enemy.CurrentNode.Name}");
                movement.SetTarget(enemy.CurrentNode);
            }
        }
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
            attack.Initialize(combatantType);
        }
        else
        {
            Debug.LogWarning("Wrong unit type on Combatant");
        }

            movement.Initialize(_pathfinder, this, moveSpeed);
    }

    public void SetTarget(GridNode targetNode)
    {
        movement.SetTarget(targetNode);
    }

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
        return UnitManager.Instance.unitsByArmy[Army.Enemy];
    }

    public void SetupMat(Material selected)
    {
        Debug.Log("Setting up materials");
        renderers = GetComponentsInChildren<Renderer>();
        Debug.Log(renderers.Length);
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
