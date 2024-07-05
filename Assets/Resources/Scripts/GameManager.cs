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
  private GameObject player;

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
      sceneController.LoadGameScene(gamesceneToLoad, (AsyncOperation sceneLoad) =>
      {
        GameObject spawnPoint = GameObject.FindGameObjectWithTag("Spawn");

        player = Instantiate(playerPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
        vcam.Follow = player.transform;
        vcam.LookAt = player.transform;
      });

    // else
    //   sceneController.LoadMainMenu((AsyncOperation sceneLoad) =>
    //   {
    //     // audioController = GetComponent<MainAudioController>();
    //     // audioController.PlayDefaultAmbiantSounds();
    //   });
  }

  public void StartGame()
  {
    // sceneController.LoadGameFromMainMenu(sceneToLoad, (AsyncOperation asyncOperation) =>
    // {
    //   audioController.PlayFirstBGM();

    //   // SPAWN PLAYER 1

    //   // Player.state.deadState.OnEnter += HandlePlayerDeath;
    //   // Player.OnCheckpointActivated += HandleCheckpointActivated;

    //   // SpawnPlayerInScene(sceneToLoad);
    // });
  }

  void HandlePlayerDeath(Player player) =>
    StartCoroutine(RespawnAfterXSecs(3));

  IEnumerator RespawnAfterXSecs(int secondsBeforeRespawn)
  {
    yield return new WaitForSeconds(secondsBeforeRespawn);
    Debug.LogError("RESPAWN - Not Implemented");
    // RESPAWN
  }

  void HandleCheckpointActivated(GameObject checkpoint)
    => latestCheckpointPosition = checkpoint.transform.position;

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
