using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeatBar : MonoBehaviour
{
  private GameManager gameManager;
  private Player player;
  [SerializeField] private Slider slider;
  [SerializeField] private GameObject burnoutText;
  [SerializeField] private GameObject burnoutTimer;
  [SerializeField] private RectTransform SliderThresholdRect;
  [SerializeField] private float heatbarLerpTime = 0.2f;

  private IEnumerator lerpHeatbarCoroutine;

  void Awake()
  {
    gameManager = FindObjectOfType<GameManager>();
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
    player.heat.BurnoutTriggered += HandleBurnout;
    slider.value = 0;
    SliderThresholdRect.anchoredPosition = new Vector2(GetThresholdXPosWithValue(), SliderThresholdRect.anchoredPosition.y);
    burnoutText.SetActive(false);
    burnoutTimer.SetActive(false);
  }

  void HandlePlayerDeath(Player p)
  {
    player.state.deadState.OnEnter -= HandlePlayerDeath;
    player.heat.HeatIncreased -= HandleHeatChange;
    player.heat.HeatDecreased -= HandleHeatChange;
    player.heat.BurnoutTriggered -= HandleBurnout;
  }

  void HandleHeatChange(int currentHeat)
  {
    if (lerpHeatbarCoroutine != null)
      StopCoroutine(lerpHeatbarCoroutine);
    lerpHeatbarCoroutine = LerpHeatbar(currentHeat);
    StartCoroutine(lerpHeatbarCoroutine);
    SliderThresholdRect.anchoredPosition = new Vector2(GetThresholdXPosWithValue(), SliderThresholdRect.anchoredPosition.y);

  }

  void HandleBurnout()
  {
    StartCoroutine(BurnoutDisplay());
  }

  IEnumerator LerpHeatbar(int newValue)
  {
    float timer = 0f;
    while (timer < heatbarLerpTime)
    {
      slider.value = Mathf.Lerp(slider.value, newValue, Time.deltaTime * 10);
      timer += Time.deltaTime;
      yield return null;
    }
    slider.value = newValue;
  }

  IEnumerator BurnoutDisplay()
  {
    burnoutText.SetActive(true);
    burnoutTimer.SetActive(true);
    float timer = player.heat.burnoutTimeUntilOver;
    TextMeshProUGUI timerText = burnoutTimer.GetComponent<TextMeshProUGUI>();
    while (timer >= 0f)
    {
      timerText.text = Mathf.RoundToInt(timer).ToString();
      if (!player || !player.heat.burnoutMode)
      {
        burnoutText.SetActive(false);
        burnoutTimer.SetActive(false);
        yield break;
      }
      timer -= Time.deltaTime;
      yield return null;
    }
  }

  private float GetThresholdXPosWithValue()
  {
    return slider.GetComponent<RectTransform>().rect.width * player.heat.burnoutRecoveryThreshold / player.heat.maxHeat;
  }
}
