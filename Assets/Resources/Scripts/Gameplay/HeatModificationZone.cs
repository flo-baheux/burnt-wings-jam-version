using System.Collections;
using UnityEngine;

public class HeatModificationZone : MonoBehaviour
{
  [Range(-100, 100)]
  [SerializeField] private int heatModifier;
  [SerializeField] private int ticksEveryS = 2;

  private Player playerInZone = null;

  public void OnTriggerEnter2D(Collider2D other)
  {
    other.gameObject.TryGetComponent(out Player player);
    if (player)
      playerInZone = player;
  }

  public void OnTriggerExit2D(Collider2D other)
  {
    other.gameObject.TryGetComponent(out Player player);
    if (player)
      playerInZone = null;
  }

  void Start()
  {
    InvokeRepeating("DealDamageIfPlayerInZone", ticksEveryS, ticksEveryS);
  }

  public void DealDamageIfPlayerInZone()
  {
    if (!playerInZone)
      return;
    if (heatModifier > 0)
      playerInZone.heat.IncreaseHeat(heatModifier);
    else
      playerInZone.heat.DecreaseHeat(heatModifier);
  }
}
