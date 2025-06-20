using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS_1333
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Selector selector;

        private void Awake()
        {
            GridManager.Instance.InitializeGrid();
            selector.Initialize();

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}