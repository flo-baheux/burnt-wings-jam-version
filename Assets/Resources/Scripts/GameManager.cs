using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[Serializable]
public struct WorldLevels
{
  public int worldId;
  public int nbLevels;
};

public class GameManager : MonoBehaviour
{
  // CAMERAS
  [SerializeField] private CinemachineVirtualCamera vcam;

  // CHARACTERS
  [SerializeField] private GameObject playerPrefab;
  public Player player;
  public int dashHeatCost = 35;

  // GAMEPLAY

  // WORLDS & LEVELS
  [SerializeField] List<WorldLevels> worldLevels;
  private Dictionary<int, int> nbLevelsPerWorld = new Dictionary<int, int>();
  private int currentWorld;
  private int currentLevel;


  // CONTROLLERS
  private MainAudioController audioController;
  [SerializeField] private SceneController sceneController;

  // PLAYER EVENTS
  public Action<Player> PlayerSpawned;

  bool gamePaused = false;

  public void Awake()
  {
    Application.targetFrameRate = 60;
    sceneController = new SceneController();
    sceneController.LoadLevelSelection();
    foreach (WorldLevels wl in worldLevels)
      nbLevelsPerWorld[wl.worldId] = wl.nbLevels;
  }

  private void SetupForLevel()
  {
    GameObject spawnPoint = GameObject.FindGameObjectWithTag("Spawn");
    player = Instantiate(playerPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation).GetComponent<Player>();
    PlayerSpawned?.Invoke(player);
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
      // player.controlsEnabled = false;
      sceneController.LoadPauseScene();
    }
    else
    {
      sceneController.UnloadPauseScene((AsyncOperation _) =>
      {
        Time.timeScale = 1;
        // player.controlsEnabled = true;
      });
    }
    gamePaused = !gamePaused;
  }

  public void StartLevel(int world, int level)
  {
    currentWorld = world;
    currentLevel = level;
    Debug.Log("STARTING LEVEL " + currentWorld + "-" + currentLevel);
    sceneController.LoadGameScene($"world_{world}_level_{level}", (AsyncOperation sceneLoad) => SetupForLevel());
  }

  public void StartNextLevel()
  {
    Destroy(player.gameObject);
    if (nbLevelsPerWorld[currentWorld] == currentLevel)
    {
      if (!nbLevelsPerWorld.ContainsKey(currentWorld + 1))
        Debug.LogError("EITHER MISTAKE OR GAME OVER");
      else
        StartLevel(currentWorld + 1, 1);
    }
    else
      StartLevel(currentWorld, currentLevel + 1);
  }
}
