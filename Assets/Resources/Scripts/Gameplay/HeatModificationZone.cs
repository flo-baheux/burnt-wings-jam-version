using System.Collections;
using UnityEngine;

public class HeatModificationZone : MonoBehaviour
{
  [Range(-100, 100)]
  [SerializeField] private int heatModifier;
  [SerializeField] private float ticksEveryS = 2;

  private Player playerInZone = null;
  Coroutine dealDamageCoroutine = null;

  public void OnTriggerEnter2D(Collider2D other)
  {
    other.gameObject.TryGetComponent(out Player player);
    if (player)
    {
      playerInZone = player;
      dealDamageCoroutine = StartCoroutine(DealDamageIfPlayerInZoneCoroutine());
    }
  }

  public void OnTriggerExit2D(Collider2D other)
  {
    other.gameObject.TryGetComponent(out Player player);
    if (player)
    {
      playerInZone = null;
      StopCoroutine(dealDamageCoroutine);
    }
  }

  IEnumerator DealDamageIfPlayerInZoneCoroutine()
  {
    while (true)
    {
      if (!playerInZone)
        yield break;


      if (heatModifier > 0)
        playerInZone.heat.IncreaseHeat(heatModifier);
      else if (heatModifier < 0)
        playerInZone.heat.DecreaseHeat(heatModifier);
      yield return new WaitForSeconds(ticksEveryS);
    }
  }
}
