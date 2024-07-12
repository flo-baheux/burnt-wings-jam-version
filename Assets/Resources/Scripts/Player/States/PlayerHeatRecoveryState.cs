using System.Collections;
using UnityEngine;

public class PlayerHeatRecoveryState : PlayerStandingState
{
  private Coroutine heatRecoveryCoroutine;

  public PlayerHeatRecoveryState(Player player) : base(player)
  {
    this.state = State.HEAT_RECOVERY;
  }

  public override void Enter()
  {
    heatRecoveryCoroutine = Player.StartCoroutine(HeatRecovery());
    base.Enter();
  }

  public override State? CustomUpdate()
  {
    if (Player.input.DashReleased || Player.input.movementVector.magnitude > 0.1f)
      return State.STANDING;

    return base.CustomUpdate();
  }

  public override void Exit()
  {
    Player.StopCoroutine(heatRecoveryCoroutine);
    base.Exit();
  }

  public IEnumerator HeatRecovery()
  {
    yield return new WaitForSeconds(Player.heat.heatRecoveryDelay);
    while (true)
    {
      Player.heat.DecreaseHeat(Player.heat.heatRecoveryPerSec / 10);
      yield return new WaitForSeconds(0.1f);
    }
  }
}