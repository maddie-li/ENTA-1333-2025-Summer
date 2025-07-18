using System.Collections;
using System.Collections.Generic;
using RTS_1333;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    public static LoadingManager Instance { get; private set; }

    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider loadingBar;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

    }
    public void LoadNewScene(int id)
    {
        loadingScreen.SetActive(true);
        loadingBar.value = 0;
        StartCoroutine(SwitchToSceneAsync(id));
    }

    IEnumerator SwitchToSceneAsync(int id)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(id);
        while (!asyncLoad.isDone)
        {
            loadingBar.value = asyncLoad.progress;
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        loadingScreen.SetActive(false);

        if(id == 1)
        {
            GameManager.Instance.SetupGame();
        }
    }
}
