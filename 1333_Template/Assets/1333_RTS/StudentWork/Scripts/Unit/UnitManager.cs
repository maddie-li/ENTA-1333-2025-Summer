using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace RTS_1333
{
    public class UnitManager : MonoBehaviour
    {
        [SerializeField] private GridManager gridManager;
        [SerializeField] private Pathfinder pathfinder;
        [SerializeField] private GameObject prefab;
        [SerializeField] private Vector2Int[] nodePosition;

        public List<BaseUnit> allUnits = new();   

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
                TestSpawn();
        }

        private void TestSpawn()
        {
            foreach (Vector2Int pos in nodePosition)
            {
                GridNode node = gridManager.GetNode(pos);
                if (node == null || node.CurrentUnit != null) return;

                GameObject unitObject = Instantiate(prefab);
                UnitInstance unit = unitObject.GetComponent<UnitInstance>();

                if (unit != null)
                {
                    RegisterUnit(unit);
                    unit.Initialize(pathfinder, gridManager);
                    unit.SetNodePos(node);
                }
            }

            
        }

        public void RegisterUnit(BaseUnit unit)
        {
            if (unit != null && !allUnits.Contains(unit))
            {
                allUnits.Add(unit);
            }
        }
        public void UnregisterUnit(BaseUnit unit)
        {
            allUnits.Remove(unit);
        }
    }
}
