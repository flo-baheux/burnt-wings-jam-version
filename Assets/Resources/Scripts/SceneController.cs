using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController
{
  private bool IsLoading = false;
  private Action<AsyncOperation> SetIsLoadingFalseCallback;

  public SceneController() =>
    SetIsLoadingFalseCallback = (AsyncOperation _) => IsLoading = false;


  public void LoadLevelSelection(bool setActive = true, Action<AsyncOperation> callback = null)
  {
    if (IsLoading) return;
    IsLoading = true;
    AsyncOperation asyncOp = SceneManager.LoadSceneAsync("LevelSelection", LoadSceneMode.Additive);
    asyncOp.completed += callback;
    asyncOp.completed += SetIsLoadingFalseCallback;
    asyncOp.completed +=
      (AsyncOperation asyncOp) =>
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("LevelSelection"));
  }

  public void LoadMainMenu(Action<AsyncOperation> callback = null)
  {
    if (IsLoading) return;
    IsLoading = true;
    AsyncOperation asyncOp = SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Additive);
    asyncOp.completed += callback;
    asyncOp.completed += SetIsLoadingFalseCallback;
  }

  public void UnloadMainMenu(Action<AsyncOperation> callback = null)
  {
    if (IsLoading) return;
    IsLoading = true;
    AsyncOperation asyncOp = SceneManager.UnloadSceneAsync("MainMenu");
    asyncOp.completed += callback;
    asyncOp.completed += SetIsLoadingFalseCallback;
  }

  public void LoadGameScene(string sceneName, Action<AsyncOperation> callback = null)
  {
    if (IsLoading) return;
    IsLoading = true;


    AsyncOperation asyncOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    asyncOp.completed +=
    (AsyncOperation asyncOp) =>
    {
      if (SceneManager.GetActiveScene().name == "LevelSelection")
        SceneManager.UnloadSceneAsync("LevelSelection");
      SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
      SceneManager.LoadScene("IngameHUD", LoadSceneMode.Additive);
    };
    asyncOp.completed += callback;
    asyncOp.completed += SetIsLoadingFalseCallback;
  }

  public void ReloadCurrentScene(Action<AsyncOperation> callback = null)
  {
    string currentSceneName = SceneManager.GetActiveScene().name;
    if (IsLoading) return;
    IsLoading = true;
    SceneManager.UnloadSceneAsync(currentSceneName);
    AsyncOperation asyncOp = SceneManager.LoadSceneAsync(currentSceneName, LoadSceneMode.Additive);
    asyncOp.completed += callback;
    asyncOp.completed +=
      (AsyncOperation asyncOp) =>
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(currentSceneName));
    asyncOp.completed += SetIsLoadingFalseCallback;
  }

  public void LoadPauseScene(Action<AsyncOperation> callback = null)
    => SceneManager.LoadSceneAsync("IngameMenu", LoadSceneMode.Additive).completed += callback;

  public void UnloadPauseScene(Action<AsyncOperation> callback = null)
    => SceneManager.UnloadSceneAsync("IngameMenu").completed += callback;

  public void UnloadIngameHUD(Action<AsyncOperation> callback = null)
    => SceneManager.UnloadSceneAsync("IngameHUD").completed += callback;
}