using System.Collections.Generic;
using Mono.Cecil;
using NUnit.Framework;
using UnityEngine;

namespace RTS_1333
{
    public class UnitManager : MonoBehaviour
    {
        [SerializeField] private Pathfinder pathfinder;
        [SerializeField] private GameObject prefab;
        [Header("Visuals")]
        [SerializeField] private Material selectedMat;
        [Header("Testing")]
        [SerializeField] private Vector2Int[] nodePosition;

        public Dictionary<Army, List<UnitInstance>> unitsByArmy = new();
        public List<UnitInstance> allUnits = new();

        public static UnitManager Instance;
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            unitsByArmy.Add(Army.Player, new List<UnitInstance>());
            unitsByArmy.Add(Army.Enemy, new List<UnitInstance>());
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
            }

            
        }

        public UnitInstance SpawnUnit(GameObject prefab, Vector3 pos)
        {
            GridNode node = GridManager.Instance.GetNodeFromWorldPosition(pos);
            if (node == null || node.CurrentUnit != null) return null;

            GameObject unitObject = Instantiate(prefab, this.transform);
            UnitInstance unit = unitObject.GetComponent<UnitInstance>();

            if (unit != null)
            {
                RegisterUnit(unit);
                unit.Initialize(pathfinder);
                unit.SetNodePos(node);
                unit.SetupMat(selectedMat);
                Debug.Log("Initialised new unit");

                return unit;
            }

            return null;
        }

        public void RegisterUnit(UnitInstance unit)
        {
            // add mainlist
            if (unit != null && !allUnits.Contains(unit))
            {
                allUnits.Add(unit);
            }
            // add armylist
            if (!unitsByArmy[unit.army].Contains(unit))
            {
                unitsByArmy[unit.army].Add(unit);
            }
        }
        public void UnregisterUnit(UnitInstance unit)
        {
            allUnits.Remove(unit);

            if (unitsByArmy.TryGetValue(unit.army, out var unitList))
            {
                unitList.Remove(unit);
            }
        }
    }
}
