using System;
using System.Collections.Generic;
using UnityEngine;

public class WorldLevelSelect : MonoBehaviour
{
  private List<GameObject> worldPanels = new List<GameObject>();
  private int currentPanelIndex = 0;
  private enum WorldSelectNavigationDirection
  {
    PREVIOUS,
    NEXT
  }

  void Start()
  {
    for (int i = 0; i < transform.childCount; i++)
    {
      GameObject panel = transform.GetChild(i).gameObject;
      panel.SetActive(false);
      worldPanels.Add(panel);
    }
    worldPanels[0].SetActive(true);
  }

  private void MoveToPanelOnClick(WorldSelectNavigationDirection direction)
  {
    worldPanels[currentPanelIndex].SetActive(false);
    currentPanelIndex += direction == WorldSelectNavigationDirection.PREVIOUS ? -1 : 1;
    Math.Clamp(currentPanelIndex, 0, worldPanels.Count - 1);
    worldPanels[currentPanelIndex].SetActive(true);
  }

  public void OnClickPreviousWorld() => MoveToPanelOnClick(WorldSelectNavigationDirection.PREVIOUS);
  public void OnClickNextWorld() => MoveToPanelOnClick(WorldSelectNavigationDirection.NEXT);
}
