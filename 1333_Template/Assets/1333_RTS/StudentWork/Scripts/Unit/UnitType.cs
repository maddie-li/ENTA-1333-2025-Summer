using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RTS_1333
{
    [CreateAssetMenu(fileName = "UnitType", menuName = "Game/UnitType")]
    public class UnitType : ScriptableObject
    {
        [SerializeField] private int width = 1;
        [SerializeField] private int length = 1;

        [SerializeField] private int maxHP = 1;
        [SerializeField] private float moveSpeed = 1;
        [SerializeField] private int damage = 1;
        [SerializeField] private int defense = 1;
        [SerializeField] private AttackType attackType = AttackType.Melee;
        [SerializeField] private int range = 1;

        [SerializeField] private GameObject unitPrefab;

        public int Width => width;
        public int Length => length;

        public int MaxHP => maxHP;
        public float MoveSpeed => moveSpeed;
        public int Damage => damage;
        public int Defense => defense;

        public AttackType AttackType => attackType;


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
