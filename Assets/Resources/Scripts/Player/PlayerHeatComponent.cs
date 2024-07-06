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
  public int maxHeat = 3;
  public int currentHeat = 0;
  public float overheatTimeBeforeBurnout = 3f;
  public bool overheatMode = false;

  private void Awake()
  {
    currentHeat = minHeat;
    player = GetComponent<Player>();
  }

  // Increase heat. However if the heat reaches max, start overheat mode.
  // If the heat increases further, burnout is triggered.
  public void IncreaseHeat()
  {
    currentHeat = Math.Clamp(currentHeat + 1, minHeat, maxHeat);
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
  public void DecreaseHeat()
  {
    currentHeat = Math.Clamp(currentHeat - 1, minHeat, maxHeat);
    HeatDecreased?.Invoke(currentHeat);
    if (overheatMode)
      overheatMode = false;
    minHeat = Math.Clamp(minHeat + 1, 0, maxHeat);
    if (minHeat == maxHeat)
      burnoutTriggered?.Invoke();
  }

  // Wait x sec - if still in overheat, triggers a burnout
  private IEnumerator Overheat()
  {
    Debug.Log("OVERHEAT OMG");
    yield return new WaitForSeconds(overheatTimeBeforeBurnout);
    if (overheatMode)
      burnoutTriggered?.Invoke();
  }
}
