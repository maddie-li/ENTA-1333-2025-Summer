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
        [Header("Visuals")]
        [SerializeField] private Material selectedMat;
        [Header("Testing")]
        [SerializeField] private Vector2Int[] nodePosition;

        public List<BaseUnit> allUnits = new();

        public static UnitManager Instance;
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
                TestSpawn();
        }

        private void TestSpawn()
        {
            foreach (Vector2Int pos in nodePosition)
            {
                Vector3 spawnPos = new Vector3(pos.x, pos.y, 0);

                SpawnUnit(prefab,spawnPos);

               /* GridNode node = gridManager.GetNode(pos);
                if (node == null || node.CurrentUnit != null) return;

                GameObject unitObject = Instantiate(prefab);
                UnitInstance unit = unitObject.GetComponent<UnitInstance>();

                if (unit != null)
                {
                    RegisterUnit(unit);
                    unit.Initialize(pathfinder, gridManager);
                    unit.SetNodePos(node);
                }*/
            }

            
        }

        public UnitInstance SpawnUnit(GameObject prefab, Vector3 pos)
        {
            GridNode node = gridManager.GetNodeFromWorldPosition(pos);
            if (node == null || node.CurrentUnit != null) return null;

            GameObject unitObject = Instantiate(prefab, this.transform);
            UnitInstance unit = unitObject.GetComponent<UnitInstance>();

            if (unit != null)
            {
                RegisterUnit(unit);
                unit.Initialize(pathfinder, gridManager);
                unit.SetNodePos(node);
                unit.SetupMat(selectedMat);
                Debug.Log("Initialised new unit");

                return unit;
            }

            return null;
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
