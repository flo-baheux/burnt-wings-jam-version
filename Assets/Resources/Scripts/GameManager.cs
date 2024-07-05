using System.Collections;
using Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
  // CAMERAS
  [SerializeField] private CinemachineVirtualCamera vcam;

  // SCENES SETUP
  [SerializeField] private string gamesceneToLoad;

  // CHARACTERS
  [SerializeField] private GameObject playerPrefab;
  private Player player;

  // GAMEPLAY
  private Vector2 latestCheckpointPosition;

  // CONTROLLERS
  private MainAudioController audioController;
  [SerializeField] private SceneController sceneController;

  bool gamePaused = false;

  public void Awake()
  {
    sceneController = new SceneController();
    if (gamesceneToLoad != null)
      sceneController.LoadGameScene(gamesceneToLoad, (AsyncOperation sceneLoad) => SetupForLevel());

    // else
    //   sceneController.LoadMainMenu((AsyncOperation sceneLoad) =>
    //   {
    //     // audioController = GetComponent<MainAudioController>();
    //     // audioController.PlayDefaultAmbiantSounds();
    //   });
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
    Debug.Log("PLAYER DEAD - MORT MORT MORT");
    player.state.deadState.OnEnter -= HandlePlayerDeath;
    Destroy(player);
    sceneController.ReloadCurrentScene((AsyncOperation sceneLoad) => SetupForLevel());
  }

  public void SetLatestCheckpoint(Checkpoint checkpoint)
  {
    Debug.Log("Checkpoint saved");
    latestCheckpointPosition = checkpoint.transform.position;
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
}
