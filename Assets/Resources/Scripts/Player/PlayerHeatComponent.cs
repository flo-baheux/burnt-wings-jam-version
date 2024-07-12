using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerHeatComponent : MonoBehaviour
{
  public Action<int> HeatIncreased;
  public Action<int> HeatDecreased;
  public Action BurnoutCancelled;
  public Action BurnoutTriggered;
  public Action BurnoutOver;

  public int minHeat = 0, currentHeat = 0, maxHeat = 100;
  public int burnoutRecoveryThreshold = 70;
  public int burnoutRecoveryThresholdDecreaseStep = 5;
  public float burnoutTimeUntilOver = 5;
  public int heatRecoveryPerSec = 40;
  public float heatRecoveryDelay = 0.3f;

  public bool burnoutMode = false;

  private void Awake()
  {
    currentHeat = minHeat;
  }

  public void IncreaseHeat(int value)
  {
    currentHeat = Math.Clamp(currentHeat + value, minHeat, maxHeat);
    HeatIncreased?.Invoke(currentHeat);

    if (!burnoutMode && currentHeat == maxHeat)
      StartCoroutine(Burnout());
  }

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
