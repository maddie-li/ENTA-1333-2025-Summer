using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RTS_1333
{
    public class UnitType : ScriptableObject
    {
        [SerializeField] private int width = 1;
        [SerializeField] private int length = 1;

        [SerializeField] private int maxHP = 1;

        [SerializeField] protected GameObject unitPrefab;

        [SerializeField] private Army army;

        public int Width => width;
        public int Length => length;

        public int MaxHP => maxHP;

        public GameObject UnitPrefab => unitPrefab;

        public Army Army => army;


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
