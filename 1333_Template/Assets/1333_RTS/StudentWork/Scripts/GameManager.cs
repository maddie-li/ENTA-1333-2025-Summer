using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;
using static InputSystem_Actions;

namespace RTS_1333
{
    public enum TimeState
    {
        Paused = 0,
        Normal = 1,
        Fast = 2,
        SuperFast = 10
    }

    public enum GameState
    {
        Menu,     
        Gameplay,  
        Paused,    
        Victory,  
        Defeat    
    }
    public class GameManager : MonoBehaviour
    {
        private InputSystem_Actions interactActions;
        public static GameManager Instance { get; private set; }

        public GameState currentState = GameState.Menu;

        [SerializeField] private GameObject[] managerPrefabs;
        private List<GameObject> destroyableManagers = new List<GameObject>();

        private TimeState currentTimeState = TimeState.Normal;
        private void OnEnable()
        {
            interactActions.Enable();
        }

        private void OnDisable()
        {
            interactActions.Disable();
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            interactActions = new InputSystem_Actions();

        }

        void Update()
        {
            if (currentState == GameState.Gameplay)
            {
                CheckLoseCondition();

                if (interactActions.Game.Pause.WasPressedThisFrame())
                {
                    SetGameState(GameState.Paused);
                    Debug.LogError("Pausing game");
                }
            }
            else if (currentState == GameState.Paused)
            {
                if (interactActions.Game.Pause.WasPressedThisFrame())
                {
                    SetGameState(GameState.Gameplay);
                    Debug.LogError("Resumiing game");
                }
            }
        }
        public void SetGameState(GameState newState)
        {
            currentState = newState;
            Debug.Log($"Game state changed to: {newState}");

            switch (newState)
            {
                case GameState.Menu:
                    UIManager.Instance.ForceTimescale(TimeState.Paused);
                    break;

                case GameState.Gameplay:
                    UIManager.Instance.ForceTimescale(TimeState.Normal);
                    break;

                case GameState.Paused:
                    UIManager.Instance.ForceTimescale(TimeState.Paused);
                    break;

                case GameState.Victory:
                    UIManager.Instance.ForceTimescale(TimeState.Paused);
                    UIManager.Instance.SetUIScreen(UIManager.UIScreen.Victory);
                    break;

                case GameState.Defeat:
                    UIManager.Instance.ForceTimescale(TimeState.Paused);
                    UIManager.Instance.SetUIScreen(UIManager.UIScreen.Defeat);
                    break;

            }
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
                destroyableManagers.Add(manager);
            }

            GridManager.Instance.InitializeGrid();
            UIManager.Instance.ForceTimescale(TimeState.Normal);

            SetGameState(GameState.Gameplay);
            
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
            if (currentState == GameState.Gameplay || currentState == GameState.Paused)
            {
                switch (army)
                {
                    case Army.Player:
                        Debug.Log("Player lost");
                        SetGameState(GameState.Defeat);
                        break;
                    case Army.Enemy:
                        Debug.Log("Player won");
                        SetGameState(GameState.Victory);
                        break;
                }
            }
            
        }

        public void HandleResetGame()
        {
            /*UIManager.Instance.ForceTimescale(TimeState.Normal);
            UIManager.Instance.SetUIScreen(UIManager.UIScreen.Game);

            foreach (GameObject manager in destroyableManagers)
            {
                Debug.Log($"Destroying: {manager.name}");
                Destroy(manager);
            }

            destroyableManagers.Clear();
            SetupGame();*/
        }
    }
}