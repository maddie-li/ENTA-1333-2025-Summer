using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace RTS_1333
{
    public class UnitManager : MonoBehaviour
    {
        public Pathfinder Pathfinder;

        public Material[] combatantMaterials = new Material[5]; // player, enemy, valid, invalid, selected
        public Material[] buildingMaterials = new Material[5]; 

        public Dictionary<Army, List<Unit>> UnitsByArmy = new();
        public List<Unit> allUnits = new();
        public Dictionary<Army, bool> armyActivated = new();

        public static UnitManager Instance;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            foreach (Army army in System.Enum.GetValues(typeof(Army)))
            {
                UnitsByArmy[army] = new List<Unit>();
                armyActivated[army] = false;
            }
        }

        public Unit SpawnUnit(GameObject prefab, Vector3 position, Army army)
        {
            GridNode node = GridManager.Instance.GetNodeFromWorldPosition(position);
            if (node == null || node.CurrentUnit != null) return null;

            GameObject obj = Instantiate(prefab, transform);
            Unit Unit = obj.GetComponent<Unit>();

            if (Unit == null) return null;

            RegisterUnit(Unit);

            Unit.SetNodePos(node);

            Unit.SetupMat();

            armyActivated[army] = true;

            return Unit;
        }

        public void RegisterUnit(Unit Unit)
        {
            // add mainlist
            if (Unit != null && !allUnits.Contains(Unit))
            {
                allUnits.Add(Unit);
            }
            // add armylist
            if (!UnitsByArmy[Unit.Army].Contains(Unit))
            {
                UnitsByArmy[Unit.Army].Add(Unit);
            }
        }
        public void UnregisterUnit(Unit Unit)
        {
            allUnits.Remove(Unit);

            if (UnitsByArmy.TryGetValue(Unit.Army, out var UnitList))
            {
                UnitList.Remove(Unit);
            }
        }

        public void Reset()
        {
            foreach (Army army in (Army[])System.Enum.GetValues(typeof(Army)))
            {
                armyActivated[army] = false;
            }
        }
    }
}
