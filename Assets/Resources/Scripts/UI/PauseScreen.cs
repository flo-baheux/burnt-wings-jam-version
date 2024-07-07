using UnityEngine;

public class PauseScreen : MonoBehaviour
{
  private GameManager gameManager;

  void Awake()
    => gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

  public void OnClickResume() => gameManager.PauseResumeGame();

  public void OnClickQuitGame()
  {
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#endif
    Application.Quit();
  }
}
