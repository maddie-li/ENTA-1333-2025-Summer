using System.Collections.Generic;
using UnityEngine;

namespace RTS_1333
{
    public class UnitManager : MonoBehaviour
    {
        [SerializeField] private Pathfinder pathfinder;
        [SerializeField] private GameObject prefab;
        [Header("Visuals")]
        [SerializeField] private Material selectedMat;
        public Material EnemyMat;
        public Material PlayerMat;
        [Header("Testing")]
        [SerializeField] private Vector2Int[] nodePosition;

        public Dictionary<Army, List<Combatant>> unitsByArmy = new();
        public List<Combatant> allUnits = new();

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

            unitsByArmy.Add(Army.Player, new List<Combatant>());
            unitsByArmy.Add(Army.Enemy, new List<Combatant>());

            foreach (Army army in (Army[])System.Enum.GetValues(typeof(Army)))
            {
                armyActivated[army] = false;
            }
        }

        public Combatant SpawnUnit(GameObject prefab, Vector3 pos)
        {
            GridNode node = GridManager.Instance.GetNodeFromWorldPosition(pos);
            if (node == null || node.CurrentUnit != null) return null;

            GameObject unitObject = Instantiate(prefab, this.transform);
            Combatant unit = unitObject.GetComponent<Combatant>();

            if (unit != null)
            {
                RegisterUnit(unit);
                unit.Initialize(pathfinder);
                unit.SetNodePos(node);

                Material defaultMat = null;

                switch (unit.army)
                {
                    case Army.Player:
                        defaultMat = PlayerMat;
                        break;
                    case Army.Enemy:
                        defaultMat = EnemyMat;
                        break;
                }

                if (defaultMat != null) unit.SetupMat(defaultMat, selectedMat);

                armyActivated[unit.army] = true;
                //Debug.Log("Initialised new unit");

                return unit;
            }

            return null;
        }

        public void RegisterUnit(Combatant unit)
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
        public void UnregisterUnit(Combatant unit)
        {
            allUnits.Remove(unit);

            if (unitsByArmy.TryGetValue(unit.army, out var unitList))
            {
                unitList.Remove(unit);
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
