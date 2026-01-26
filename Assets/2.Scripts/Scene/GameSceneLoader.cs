using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public interface ILoadProgress
{
    void UpdateProgress(float value01);
    void UpdateMessage(string message);
}

public class GameSceneLoader : MonoBehaviour, ILoadProgress
{
    [SerializeField] private Image progressBar;
    [SerializeField] private TextMeshProUGUI statusText;

    [SerializeField] protected string nextScene;

    private const float sceneWeight = 0.5f;
    private const float bootstrapWeight = 0.5f;

    public void LoadScene(bool load = false)
    {
        progressBar.fillAmount = 0f;
        StartCoroutine(LoadingScene(load));
    }

    private IEnumerator LoadingScene(bool load = false)
    {
        UpdateMessage("게임 화면 로딩중");

        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive);
        op.allowSceneActivation = false;

        while (op.progress < 0.9f)
        {
            progressBar.fillAmount = op.progress * sceneWeight;
            yield return null;
        }

        //씬 로딩 완료
        progressBar.fillAmount = sceneWeight;
        op.allowSceneActivation = true;
        while (!op.isDone) 
            yield return null;


        // 이제 씬이 로드됨
        Scene gameScene = SceneManager.GetSceneByName(nextScene);
        if (!gameScene.IsValid() || !gameScene.isLoaded)
        {
            Debug.LogError("GameScene not loaded/activated yet");
            yield break;
        }


        // 씬 담당 찾기
        IBootstraper bootstraper = null;
        foreach (GameObject root in gameScene.GetRootGameObjects())
        {
            bootstraper = root.GetComponentInChildren<IBootstraper>();
            if (bootstraper != null)
                break;
        }

        if (bootstraper != null)
        {
            yield return bootstraper.Initialize(this);

            progressBar.fillAmount = 1f;
        }

        SceneManager.SetActiveScene(gameScene);
        yield return SceneManager.UnloadSceneAsync(gameObject.scene);
    }

    public void UpdateProgress(float value)
    {
        progressBar.fillAmount = sceneWeight + (bootstrapWeight * value);
    }

    public void UpdateMessage(string message)
    {
        statusText.text = message;
    }
}