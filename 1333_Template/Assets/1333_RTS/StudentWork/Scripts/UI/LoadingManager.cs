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

    [SerializeField] private float endFilLDuration = 1f;


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
        //Debug.Log($"Loading scene: {id}");
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

        yield return StartCoroutine(FakeLoadingBarFill());
        loadingScreen.SetActive(false);

        if(id == 1)
        {
            GameManager.Instance.SetupGame();
        }
    }

    IEnumerator FakeLoadingBarFill()
    {
        float duration = endFilLDuration;
        float elapsed = 0f;
        float startValue = loadingBar.value;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            loadingBar.value = Mathf.Lerp(startValue, 1f, elapsed / duration);
            yield return null;
        }

        loadingBar.value = 1f;
    }
}
