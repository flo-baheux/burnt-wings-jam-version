using System.Collections;
using Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
  // CAMERAS
  [SerializeField] private CinemachineVirtualCamera vcam;

  // SCENES SETUP
  // [SerializeField] private string gamesceneToLoad;

  // CHARACTERS
  [SerializeField] private GameObject playerPrefab;
  private Player player;

  // GAMEPLAY

  // CONTROLLERS
  private MainAudioController audioController;
  [SerializeField] private SceneController sceneController;

  bool gamePaused = false;

  public void Awake()
  {
    sceneController = new SceneController();
    sceneController.LoadLevelSelection();
  }

  private void SetupForLevel()
  {
    GameObject spawnPoint = GameObject.FindGameObjectWithTag("Spawn");
    player = Instantiate(playerPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation).GetComponent<Player>();
    vcam.Follow = player.transform;
    vcam.LookAt = player.transform;
    player.state.deadState.OnEnter += HandlePlayerDeath;
  }

  public void StartGame()
  {
    // sceneController.LoadGameFromMainMenu(sceneToLoad, (AsyncOperation asyncOperation) =>
    // {
    //   audioController.PlayFirstBGM();

    //   // SPAWN PLAYER 1


    //   // Player.OnCheckpointActivated += HandleCheckpointActivated;

    //   // SpawnPlayerInScene(sceneToLoad);
    // });
  }

  void HandlePlayerDeath(Player player)
  {
    player.state.deadState.OnEnter -= HandlePlayerDeath;
    Destroy(player.gameObject);
    sceneController.ReloadCurrentScene((AsyncOperation sceneLoad) => SetupForLevel());
  }

  public void PauseResumeGame()
  {
    if (!gamePaused)
    {
      Time.timeScale = 0;
      // Player.controlsEnabled = false;
      sceneController.LoadPauseScene();
    }
    else
    {
      sceneController.UnloadPauseScene((AsyncOperation _) =>
      {
        Time.timeScale = 1;
        // Player.controlsEnabled = true;
      });
    }
    gamePaused = !gamePaused;
  }

  public void StartLevel(int world, int level)
  {
    sceneController.LoadGameScene($"world_{world}_level_{level}", (AsyncOperation sceneLoad) => SetupForLevel());
  }
}
