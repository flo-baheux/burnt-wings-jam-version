using UnityEngine;

public class PlayerJumpingState : PlayerState
{
  public PlayerJumpingState(Player player) : base(player)
  {
    this.state = State.JUMPING;
  }

  public override State? CustomUpdate()
  {
    if (Player.IsGrounded() && Player.rigidBody.velocity.y <= 0.1f)
      return State.GROUNDED;

    if (Player.controlsEnabled)
    {
      if (Player.playerInput.actions["Jump"].WasReleasedThisFrame())
        Player.rigidBody.velocity = new Vector2(Player.rigidBody.velocity.x, -Player.FallSpeed);
    }

    Player.rigidBody.velocity = new Vector2(Player.rigidBody.velocity.x, Mathf.Max(Player.rigidBody.velocity.y, -Player.FallSpeed));

    return null;
  }
}