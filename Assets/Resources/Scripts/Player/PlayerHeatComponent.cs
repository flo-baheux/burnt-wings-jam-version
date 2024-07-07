using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerHeatComponent : MonoBehaviour
{
  private Player player;

  public Action<int> HeatIncreased;
  public Action<int> HeatDecreased;
  public Action BurnoutCancelled;
  public Action BurnoutTriggered;
  public Action BurnoutOver;

  public int minHeat = 0;
  public int maxHeat = 100;
  public int burnoutRecoveryThreshold = 80;
  public int burnoutRecoveryThresholdDecreaseStep = 5;
  public int currentHeat = 0;
  public float burnoutTimeUntilOver = 3;
  public bool burnoutMode = false;

  private void Awake()
  {
    currentHeat = minHeat;
    player = GetComponent<Player>();
    BurnoutTriggered += () => Debug.Log("Burnout Triggered");
  }

  private void Update()
  {
    player.particleEmission.enabled = currentHeat != 0;
    if (currentHeat < burnoutRecoveryThreshold)
      player.particleEmission.rateOverTime = 10;
    else if (!burnoutMode)
      player.particleEmission.rateOverTime = 20;
    else
      player.particleEmission.rateOverTime = 50;
  }

  // Increase heat. However if the heat reaches max, start burnout mode.
  public void IncreaseHeat(int value)
  {
    currentHeat = Math.Clamp(currentHeat + value, minHeat, maxHeat);
    HeatIncreased?.Invoke(currentHeat);

    if (!burnoutMode && currentHeat == maxHeat)
    {
      StartCoroutine(Burnout());
    }
  }

  // Decrease heat. However, also increase minHeat.
  public void DecreaseHeat(int value)
  {
    currentHeat = Math.Clamp(currentHeat - value, minHeat, maxHeat);
    HeatDecreased?.Invoke(currentHeat);

    if (burnoutMode && currentHeat <= burnoutRecoveryThreshold)
    {
      burnoutMode = false;
      BurnoutCancelled?.Invoke();
      burnoutRecoveryThreshold -= burnoutRecoveryThresholdDecreaseStep;
    }
  }

  private IEnumerator Burnout()
  {
    burnoutMode = true;
    BurnoutTriggered?.Invoke();
    float timer = 0f;
    while (timer < burnoutTimeUntilOver)
    {
      timer += Time.deltaTime;
      if (!burnoutMode)
        yield break;
      yield return null;
    }
    BurnoutOver?.Invoke();
  }

  public bool IsBeyondHeatThreshold() => currentHeat > burnoutRecoveryThreshold;
}
