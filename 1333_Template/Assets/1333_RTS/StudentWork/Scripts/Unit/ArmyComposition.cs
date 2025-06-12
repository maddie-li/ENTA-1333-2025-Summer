using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS_1333
{
    [CreateAssetMenu(fileName = "ArmyComposition", menuName = "Game/ArmyComposition")]
    public class ArmyComposition : ScriptableObject
    {
        [System.Serializable]
        public class UnitEntry
        {
            public UnitType type;
            public int count = 1;
        }

        public List<UnitEntry> units = new();

        [Header("Army Info")]
        public string armyName = "New Army";

        [Header("Units to Spawn")]
        public List<UnitEntry> unitEntries = new List<UnitEntry>();
    }
}