using UnityEngine;
using UnityEngine.UI;

public class HeatBar : MonoBehaviour
{
  private GameManager gameManager;
  private Player player;
  private Slider slider;

  void Awake()
  {
    gameManager = FindObjectOfType<GameManager>();
    slider = GetComponent<Slider>();
  }

  void Start()
  {
    gameManager.PlayerSpawned += HandlePlayerSpawn;
    player = gameManager.player;
    HandlePlayerSpawn(player);
  }

  void HandlePlayerSpawn(Player p)
  {
    player = p;
    slider.minValue = player.heat.minHeat;
    slider.maxValue = player.heat.maxHeat;
    player.state.deadState.OnEnter += HandlePlayerDeath;
    player.heat.HeatIncreased += HandleHeatChange;
    player.heat.HeatDecreased += HandleHeatChange;
    player.heat.OverheatTriggered += HandleOverheat;
    player.heat.burnoutTriggered += HandleBurnout;
    slider.value = 0;
  }

  void HandlePlayerDeath(Player p)
  {
    player.state.deadState.OnEnter -= HandlePlayerDeath;
    player.heat.HeatIncreased -= HandleHeatChange;
    player.heat.HeatDecreased -= HandleHeatChange;
    player.heat.OverheatTriggered -= HandleOverheat;
    player.heat.burnoutTriggered -= HandleBurnout;
  }

  void HandleHeatChange(int currentHeat) => slider.value = currentHeat;

  void HandleOverheat()
  {

  }

  void HandleBurnout()
  {

  }
}
