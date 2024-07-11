public class PlayerHeatRecoveryState : PlayerGroundedState
{
  private bool coolingOff = false;
  public PlayerHeatRecoveryState(Player player) : base(player)
  {
    this.state = State.HEAT_RECOVERY;
  }

  public override void Enter()
  {
    coolingOff = false;
    base.Enter();
  }

  public override State? CustomUpdate()
  {
    if (Player.input.DashReleased() || Player.input.movementVector.magnitude > 0.1f)
      return State.GROUNDED;

    if (!coolingOff)
    {
      coolingOff = true;
      Player.InvokeRepeating("CoolOff", 0, 1 / Player.heatCooldownRate);
    }
    return base.CustomUpdate();
  }

  public override void Exit()
  {
    Player.CancelInvoke("CoolOff");
    base.Exit();
  }
}