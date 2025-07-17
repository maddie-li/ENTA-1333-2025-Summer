using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS_1333
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [SerializeField] private GameObject[] managerPrefabs;

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

            foreach (GameObject prefab in managerPrefabs)
            {
                GameObject manager = Instantiate(prefab);
                manager.transform.SetParent(this.transform, false);
            }

            GridManager.Instance.InitializeGrid();
        }

        public void QuitGame()
        {
            Debug.LogError("Quit game");
            Application.Quit();
        }
    }
}