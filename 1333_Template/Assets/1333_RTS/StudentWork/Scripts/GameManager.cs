using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS_1333
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [SerializeField] private Selector selector;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

        }

        public void SetupGame()
        {
            FXManager.Instance.DoFX(FXType.Select);
            UIManager.Instance.SetUIScreen(UIManager.UIScreen.Game);

            GridManager.Instance.InitializeGrid();
            selector.Initialize();
        }

        public void QuitGame()
        {
            Debug.LogError("Quit game");
            Application.Quit();
        }
    }
}