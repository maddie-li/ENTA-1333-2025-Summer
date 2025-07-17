using System.Collections;
using System.Collections.Generic;
using RTS_1333;
using UnityEngine;

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
        DontDestroyOnLoad(gameObject); // optional
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

        UIScreens[(int)screen].SetActive(true);

        CurrentScreen = screen;
        
    }

    public void StartClicked()
    {
        GameManager.Instance.SetupGame();
    }
    
    public void QuitClicked()
    {
       
        GameManager.Instance.QuitGame();
    }
}

