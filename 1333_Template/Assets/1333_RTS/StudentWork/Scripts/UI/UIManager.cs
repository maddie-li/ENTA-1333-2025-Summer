using System.Collections;
using System.Collections.Generic;
using System.Security;
using RTS_1333;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public enum UIScreen
    {
        Menu,
        Game,
        Settings,
        Victory,
        Defeat
    }


    public TMP_Text GoldText;
    public TMP_Text VictoryMessage;
    public TMP_Text DefeatMessage;


    [SerializeField] private GameObject[] UIScreens;
    private TimeStateDropdown timescaleDropdown;

    public UIScreen CurrentScreen;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        timescaleDropdown = GetComponent<TimeStateDropdown>();
    }

    private void Start()
    {
        SetUIScreen(CurrentScreen);
    }

    public void SetUIScreen(UIScreen screen)
    {
        foreach (GameObject s in UIScreens)
        {
            s.SetActive(false);
        }

        Debug.Log($"Setting active {screen}");

        UIScreens[(int)screen].SetActive(true);

        CurrentScreen = screen;

        if(screen == UIScreen.Victory)
        {
            switch (GameManager.Instance.ReasonForLoss)
            {
                case LoseReason.NoSoldiers:
                    VictoryMessage.text = "You have destroyed all soldiers of the enemy army!";
                    break;
                case LoseReason.NoMoney:
                    VictoryMessage.text = "The enemy has run out of wealth to support their campaign!";
                    break;
                case LoseReason.NoCastle:
                    VictoryMessage.text = "You have destroyed the enemy's castle!";
                    break;
                    ;
            }
        }
        else if (screen == UIScreen.Defeat)
        {
            switch (GameManager.Instance.ReasonForLoss)
            {
                case LoseReason.NoSoldiers:
                    DefeatMessage.text = "All of your soldiers have been wiped out...";
                    break;
                case LoseReason.NoMoney:
                    DefeatMessage.text = "Your wealth has been depleted...";
                    break;
                case LoseReason.NoCastle:
                    DefeatMessage.text = "Your castle has crumbled...";
                    break;
                    ;
            }
        }

    }

    public void StartClicked()
    {
        LoadingManager.Instance.LoadNewScene(1);
    }

    public void RestartClicked()
    {
        Debug.Log("UI: Resetting game");
        GameManager.Instance.HandleResetGame();
    }

    public void ResumeClicked()
    {
        GameManager.Instance.ResumeGame();
    }

    public void QuitClicked()
    {
       
        GameManager.Instance.QuitGame();
    }

    public void ForceTimescale(TimeState state)
    {
        timescaleDropdown.ForceState(state);    
    }  


}

