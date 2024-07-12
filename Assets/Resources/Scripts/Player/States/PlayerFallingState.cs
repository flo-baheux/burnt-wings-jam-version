using UnityEngine;

public class PlayerFallingState : PlayerState
{
  public PlayerFallingState(Player player) : base(player)
  {
    this.state = State.FALLING;
  }

  public override State? CustomUpdate()
  {
    if (Player.input.JumpPressed && CanCoyoteJump())
      return State.JUMPING;

    if (Player.IsGrounded())
      return State.STANDING;

    return base.CustomUpdate();
  }

  public override State? CustomFixedUpdate()
  {
    if (Player.rigidBody.velocity.y >= 0f)
      Player.rigidBody.velocity = new Vector2(Player.rigidBody.velocity.x, 0);
    Player.rigidBody.velocity = new Vector2(Player.rigidBody.velocity.x, Mathf.Max(Player.rigidBody.velocity.y, -Player.FallSpeed));
    return base.CustomFixedUpdate();
  }

  public bool CanCoyoteJump()
    => Player.state.previousState.state == State.STANDING && Time.time <= Player.state.lastTransitionTime + Player.coyoteTime;
}