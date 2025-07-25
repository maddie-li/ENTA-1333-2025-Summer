using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RTS_1333
{
    public enum TimeState
    {
        Paused = 0,
        Normal = 1,
        Fast = 2,
        SuperFast = 10
    }

    public class GameManager : MonoBehaviour
    {
        public bool GameplayRunning = false;
        public static GameManager Instance { get; private set; }

        [SerializeField] private GameObject[] managerPrefabs;

        private TimeState currentTimeState = TimeState.Normal;

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

        void Update()
        {
            if(GameplayRunning)
            CheckLoseCondition();
        }

        public void SetTimeState(TimeState newState)
        {
            currentTimeState = newState;
            Time.timeScale = (float)newState;

            Debug.Log($"Time scale set to: {Time.timeScale}x");
        }

        public void SetupGame()
        {
            Debug.Log("Setting up game");

            //Instantiate(cameraManager);

            FXManager.Instance.DoFX(FXType.Select);
            UIManager.Instance.SetUIScreen(UIManager.UIScreen.Game);

            foreach (GameObject prefab in managerPrefabs)
            {
                GameObject manager = Instantiate(prefab);
                manager.transform.SetParent(this.transform, false);
            }

            GridManager.Instance.InitializeGrid();
            UIManager.Instance.ForceTimescale(TimeState.Normal);

            GameplayRunning = true;
            
        }

        public void QuitGame()
        {
            Debug.LogError("Quit game");
            Application.Quit();
        }

        private void CheckLoseCondition()
        {
            foreach (Army army in (Army[])System.Enum.GetValues(typeof(Army)))
            {
                if (!UnitManager.Instance.armyActivated[army])
                    continue;

                if (UnitManager.Instance.unitsByArmy[army].Count == 0)
                {
                    HandleGameOver(army);
                }
            }
        }

        private void HandleGameOver(Army army)
        {
            Debug.LogError($"GAME OVER! {army} LOST because ran out of soldiers");
        }
    }
}