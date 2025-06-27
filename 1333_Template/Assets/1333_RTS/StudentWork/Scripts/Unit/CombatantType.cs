using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RTS_1333
{
    [CreateAssetMenu(fileName = "CombatantType", menuName = "Game/CombatantType")]
    public class CombatantType : UnitType
    {
        [SerializeField] private float moveSpeed = 1;
        [SerializeField] private int damage = 1;
        [SerializeField] private int defense = 1;
        [SerializeField] private AttackType attackType = AttackType.Melee;
        [SerializeField] private int sensingRange = 1; 
        [SerializeField] private int attackRange = 1;
        [SerializeField] private float attackCooldown = 1f;

        public float MoveSpeed => moveSpeed;
        public int Damage => damage;
        public int Defense => defense;
        public int SensingRange => sensingRange;
        public int AttackRange => attackRange;
        public float AttackCooldown => attackCooldown;

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
