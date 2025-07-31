using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RTS_1333
{
    public class UnitData : ScriptableObject
    {
        [SerializeField] public UnitType unitType;

        [SerializeField] private int width = 1;
        [SerializeField] private int length = 1;

        [SerializeField] private int cost = 1;

        [SerializeField] protected GameObject unitPrefab;

        [SerializeField] private Army army;

        [SerializeField] private int maxHP = 1;

        [SerializeField] private int defense = 1;

        [Header("Behaviours")]
        public bool CombatEnabled;
        public bool PlacementEnabled;
        public bool AttackEnabled;
        public bool DamageEnabled;
        public bool MovementEnabled;
        public bool SpawningEnabled;

        public UnitType UnitType => unitType;
        public int Width => width;
        public int Length => length;
        public int Cost => cost;
        public int MaxHP => maxHP;
        public int Defense => defense;

        public GameObject UnitPrefab => unitPrefab;

        public Army Army => army;
    }
}
