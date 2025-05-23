using System.Collections;
using System.Collections.Generic;
using UnityEditor.Sprites;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;   

    [SerializeField] private GameObject prefab;
    [SerializeField] private Vector2Int nodePosition;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TestSpawn();
        }
    }

    public void TestSpawn()
    {
        GridNode node;

        node = gridManager.GetNode(nodePosition);
        SpawnUnit(prefab, node);

    }


    public void SpawnUnit(GameObject prefab, GridNode node)
    {
        if (node.CurrentUnit != null)
        {
            Debug.Log("Spawn failed, node is occupied");
            return;
        }

        GameObject unitObject = Instantiate(prefab);
        Unit unit = unitObject.GetComponent<Unit>();

        if(unit != null)
        {
            unit.Initialize(node);
        }
    }
}
