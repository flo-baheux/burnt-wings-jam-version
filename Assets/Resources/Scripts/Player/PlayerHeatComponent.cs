using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerHeatComponent : MonoBehaviour
{
  private Player player;

  public Action<int> HeatIncreased;
  public Action<int> HeatDecreased;
  public Action OverheatTriggered;
  public Action burnoutTriggered;

  public int minHeat = 0;
  public int maxHeat = 100;
  public int overheatRecoveryThreshold = 80;
  public int overheatRecoveryThresholdDecreaseStep = 5;
  public int currentHeat = 0;
  public float overheatTimeBeforeBurnout = 3;
  public bool overheatMode = false;

  private void Awake()
  {
    currentHeat = minHeat;
    player = GetComponent<Player>();
    OverheatTriggered += () => Debug.Log("Overheat Triggered");
  }

  private void Update()
  {
    player.particleEmission.enabled = currentHeat != 0;
    if (currentHeat < overheatRecoveryThreshold)
      player.particleEmission.rateOverTime = 10;
    else if (!overheatMode)
      player.particleEmission.rateOverTime = 20;
    else
      player.particleEmission.rateOverTime = 50;
  }

  // Increase heat. However if the heat reaches max, start overheat mode.
  // If the heat increases further, burnout is triggered.
  public void IncreaseHeat(int value)
  {
    currentHeat = Math.Clamp(currentHeat + value, minHeat, maxHeat);
    HeatIncreased?.Invoke(currentHeat);
    if (overheatMode)
      burnoutTriggered?.Invoke();

    if (!overheatMode && currentHeat == maxHeat)
    {
      overheatMode = true;
      OverheatTriggered?.Invoke();
      StartCoroutine(Overheat());
    }
  }

  // Decrease heat. However, also increase minHeat.
  public void DecreaseHeat(int value)
  {
    currentHeat = Math.Clamp(currentHeat - value, minHeat, maxHeat);
    HeatDecreased?.Invoke(currentHeat);

    if (overheatMode && currentHeat <= overheatRecoveryThreshold)
    {
      overheatMode = false;
      overheatRecoveryThreshold -= overheatRecoveryThresholdDecreaseStep;
    }
  }

  private IEnumerator Overheat()
  {
    float timer = 0f;
    while (timer < overheatTimeBeforeBurnout)
    {
      timer += Time.deltaTime;
      if (!overheatMode)
        yield break;
      yield return null;
    }
    burnoutTriggered?.Invoke();
  }

  public bool IsBeyondHeatThreshold() => currentHeat > overheatRecoveryThreshold;
}
