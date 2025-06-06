using System.Collections;
using System.Collections.Generic;
using UnityEditor.Sprites;
using UnityEngine;

namespace RTS_1333
{
    public class UnitSpawner : MonoBehaviour
    {
        [SerializeField] private GridManager gridManager;
        [SerializeField] private Pathfinder pathfinder;

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
            BaseUnit unit = unitObject.GetComponent<BaseUnit>();

            if (unit != null)
            {
                unit.Initialize(node);
            }

            UnitInstance unitInstance = unitObject.GetComponent<UnitInstance>();

            if (unitInstance != null)
            {
                unitInstance.Initialize(pathfinder, gridManager);
            }
        }
    }
}