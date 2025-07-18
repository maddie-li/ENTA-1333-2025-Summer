using System.Collections;
using System.Collections.Generic;
using System.Security;
using RTS_1333;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public enum UIScreen
    {
        Menu,
        Game
    }

    [SerializeField] private GameObject[] UIScreens;

    public UIScreen CurrentScreen;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
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
    
    public void QuitClicked()
    {
       
        GameManager.Instance.QuitGame();
    }
}

