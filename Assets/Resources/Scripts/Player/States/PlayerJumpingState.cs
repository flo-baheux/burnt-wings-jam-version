using UnityEngine;

public class PlayerJumpingState : PlayerState
{
  private bool cancelJump = false;

  public PlayerJumpingState(Player player) : base(player)
  {
    this.state = State.JUMPING;
  }

  public override State? CustomUpdate()
  {
    if (Player.IsGrounded() && Player.rigidBody.velocity.y <= 0.1f)
      return State.GROUNDED;

    if (Player.controlsEnabled && Player.playerInput.actions["Jump"].WasReleasedThisFrame())
      cancelJump = true;

    return base.CustomUpdate();
  }

  public override State? CustomFixedUpdate()
  {
    if (cancelJump)
    {
      cancelJump = false;
      Player.rigidBody.velocity = new Vector2(Player.rigidBody.velocity.x, -Player.FallSpeed);
    }

    Player.rigidBody.velocity = new Vector2(Player.rigidBody.velocity.x, Mathf.Max(Player.rigidBody.velocity.y, -Player.FallSpeed));
    return base.CustomFixedUpdate();
  }
}