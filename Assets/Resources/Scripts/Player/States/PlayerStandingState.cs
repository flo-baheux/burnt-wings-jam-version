using System.Collections;
using UnityEngine;

public class PlayerStandingState : PlayerState
{
  public bool wasRecentlyStanding = true;

  public PlayerStandingState(Player player) : base(player)
  {
    this.state = State.STANDING;
  }

  public override State? CustomUpdate()
  {
    if (Player.input.JumpPressed || Player.input.JumpBuffered)
      return State.JUMPING;

    if (Player.input.DashHeld && Player.input.movementVector.magnitude <= 0.1f)
      return State.HEAT_RECOVERY;

    if (!Player.IsGrounded())
      return State.FALLING;

    return null;
  }
}