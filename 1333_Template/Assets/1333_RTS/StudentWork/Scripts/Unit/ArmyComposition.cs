using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS_1333
{
    [CreateAssetMenu(fileName = "ArmyComposition", menuName = "Game/Army Composition")]
    public class ArmyComposition : MonoBehaviour
    {
        [System.Serializable]
        public class UnitEntry
        {
            //public UnitTypePrefab unitTypePrefab;
            public int count = 1;
        }

        public List<UnitEntry> units = new();

    }
}