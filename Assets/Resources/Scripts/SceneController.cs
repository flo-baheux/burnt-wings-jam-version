using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController
{
  public void LoadMainMenu(Action<AsyncOperation> callback)
  {
    SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Additive).completed += callback;
  }

  public void UnloadMainMenu(Action<AsyncOperation> callback)
  {
    SceneManager.UnloadSceneAsync("MainMenu").completed += callback;
  }

  public void LoadGameScene(string sceneName, Action<AsyncOperation> callback = null)
  {
    AsyncOperation sceneLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    sceneLoad.completed += callback;
    sceneLoad.completed +=
    (AsyncOperation sceneLoad) =>
    {
      SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
      SceneManager.LoadScene("IngameHUD", LoadSceneMode.Additive);
    };
  }

  public void LoadPauseScene(Action<AsyncOperation> callback = null)
    => SceneManager.LoadSceneAsync("IngameMenu", LoadSceneMode.Additive).completed += callback;

  public void UnloadPauseScene(Action<AsyncOperation> callback = null)
    => SceneManager.UnloadSceneAsync("IngameMenu").completed += callback;

  public void UnloadIngameHUD(Action<AsyncOperation> callback = null)
    => SceneManager.UnloadSceneAsync("IngameHUD").completed += callback;
}
