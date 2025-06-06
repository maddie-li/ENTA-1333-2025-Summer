using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS_1333
{
    public class GameManager : MonoBehaviour
    {

        [SerializeField] private GridManager gridManager;
        [SerializeField] private Selector selector;
        //[SerializeField] private UnitManager unitManager;

        private void Awake()
        {
            gridManager.InitializeGrid();
            selector.Initialize(gridManager);

            //unitManager.SpawnDummyUnit();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}