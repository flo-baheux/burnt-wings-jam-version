public class PlayerDeadState : PlayerState
{
  public PlayerDeadState(Player player) : base(player)
  {
    this.state = State.DEAD;
  }

  public override State? CustomUpdate()
  {
    // One cannot escape death by itself!
    Player.controlsEnabled = false;
    return null;
  }
}