using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS_1333
{
    public class GameManager : MonoBehaviour
    {

        [SerializeField] private GridManager gridManager;
        [SerializeField] private Selector selector;

        private void Awake()
        {
            gridManager.InitializeGrid();
            selector.Initialize(gridManager);

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}