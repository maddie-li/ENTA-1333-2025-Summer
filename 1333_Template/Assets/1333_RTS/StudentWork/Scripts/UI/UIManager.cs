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
        
    }

    public void StartClicked()
    {
        LoadingManager.Instance.LoadNewScene(1);
    }

    public void RestartClicked()
    {
        GameManager.Instance.HandleResetGame();
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

