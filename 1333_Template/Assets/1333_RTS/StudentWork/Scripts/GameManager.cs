using System.Collections.Generic;
using Unity.Burst.Intrinsics;
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

    public enum GameState
    {
        Menu,     
        Gameplay,  
        Paused,    
        Victory,  
        Defeat    
    }

    public enum LoseReason
    {
        NoSoldiers,
        NoCastle,
        NoMoney
    }

    public class GameManager : MonoBehaviour
    {
        private InputSystem_Actions interactActions;
        public static GameManager Instance { get; private set; }

        public GameState currentState = GameState.Menu;

        [SerializeField] private GameObject[] managerPrefabs;
        private List<GameObject> destroyableManagers = new List<GameObject>();

        private TimeState currentTimeState = TimeState.Normal;

        // lose conditions
        public Building EnemyCastle;
        public Building PlayerCastle;
        public LoseReason ReasonForLoss;
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
                    //Debug.LogError("Pausing game");
                }
            }
            else if (currentState == GameState.Paused)
            {
                if (interactActions.Game.Pause.WasPressedThisFrame())
                {
                    SetGameState(GameState.Gameplay);
                    //Debug.LogError("Resumiing game");
                }
            }
        }
        public void SetGameState(GameState newState)
        {
            currentState = newState;
            //Debug.Log($"Game state changed to: {newState}");

            switch (newState)
            {
                case GameState.Menu:
                    UIManager.Instance.Pause(false);
                    UIManager.Instance.SetUIScreen(UIManager.UIScreen.Menu);
                    break;

                case GameState.Gameplay:
                    UIManager.Instance.Pause(false);
                    UIManager.Instance.SetUIScreen(UIManager.UIScreen.Game);
                    Selector.Instance.Enabled = true;
                    break;

                case GameState.Paused:
                    UIManager.Instance.Pause(true);
                    UIManager.Instance.SetUIScreen(UIManager.UIScreen.Settings);
                    Selector.Instance.Enabled = false;
                    
                    break;

                case GameState.Victory:
                    UIManager.Instance.Pause(false);
                    UIManager.Instance.SetUIScreen(UIManager.UIScreen.Victory);
                    Selector.Instance.Enabled = false;

                    break;

                case GameState.Defeat:
                    UIManager.Instance.Pause(false);
                    UIManager.Instance.SetUIScreen(UIManager.UIScreen.Defeat);
                    Selector.Instance.Enabled = false;

                    break;

            }
        }

        public void SetupGame()
        {
            //Debug.Log("Setting up game");

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
            UIManager.Instance.SetTimescale((float)TimeState.Normal);

            SetGameState(GameState.Gameplay);
            
        }

        public void QuitGame()
        {
            //Debug.LogError("Quit game");
            Application.Quit();
        }

        private void CheckLoseCondition()
        {
            foreach (Army army in (Army[])System.Enum.GetValues(typeof(Army)))
            {
                if (!UnitManager.Instance.armyActivated[army])
                    continue;

               /*if (UnitManager.Instance.unitsByArmy[army].Count == 0)
                {
                    HandleGameOver(army, LoseReason.NoSoldiers);
                }*/


                if (EnemyCastle == null) HandleGameOver(Army.Enemy, LoseReason.NoCastle);
                if (PlayerCastle == null) HandleGameOver(Army.Player, LoseReason.NoCastle);
/*
                if (CurrencyManager.Instance.GetGold(Army.Player) < 1) HandleGameOver(Army.Player, LoseReason.NoMoney);
                if (CurrencyManager.Instance.GetGold(Army.Player) < 1) HandleGameOver(Army.Player, LoseReason.NoMoney);*/
            }

        }

        private void HandleGameOver(Army army, LoseReason reason)
        {
            if (currentState == GameState.Gameplay || currentState == GameState.Paused)
            {

                ReasonForLoss = reason;

                switch (army)
                {
                    case Army.Player:
                        //Debug.Log("Player lost");
                        SetGameState(GameState.Defeat);
                        break;
                    case Army.Enemy:
                        //Debug.Log("Player won");
                        SetGameState(GameState.Victory);
                        break;
                }
            }
            
        }

        public void ResumeGame()
        {

            SetGameState(GameState.Gameplay);
            
        }

        public void PauseGame()
        {

            SetGameState(GameState.Paused);
        }

        public void HandleResetGame()
        {
            UnitManager.Instance.Reset();

            foreach (GameObject manager in destroyableManagers)
            {
                //Debug.Log($"Destroying: {manager.name}");
                Destroy(manager);
            }

            //Debug.Log("Resetting game");
            SceneManager.LoadScene(1);

            SetupGame();
        }
    }
}