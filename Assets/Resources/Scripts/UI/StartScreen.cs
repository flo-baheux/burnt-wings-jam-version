using UnityEngine;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour
{
  [SerializeField] private Button PlayButton;
  [SerializeField] private GameObject creditsScreen;

  private void OnEnable() => PlayButton.Select();
  private void Start() => PlayButton.Select();

  public void OnClickPlay()
  {
    GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    if (!gameManager)
      return;
    gameManager.StartGame();
  }

  public void OnClickCreditsScreen()
  {
    creditsScreen.SetActive(true);
    gameObject.SetActive(false);
  }

  public void OnClickQuitGame()
  {
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#endif
    Application.Quit();
  }
}
