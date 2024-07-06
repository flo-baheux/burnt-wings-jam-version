using UnityEngine;

public class PlayerGroundedState : PlayerState
{
  public PlayerGroundedState(Player player) : base(player)
  {
    this.state = State.GROUNDED;
  }

  public override State? CustomUpdate()
  {
    bool jumpPressed = Player.playerInput.actions["Jump"].WasPressedThisFrame();
    bool dashPressed = Player.playerInput.actions["Dash"].WasPressedThisFrame();

    if (Player.controlsEnabled && jumpPressed)
    {
      float jumpForce = Mathf.Sqrt(Player.JumpHeight * -2 * (Physics2D.gravity.y * Player.rigidBody.gravityScale));
      // Using AddForce (applying force once) is fine in update method - don't move it out
      Player.rigidBody.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
      return State.JUMPING;
    }

    if (Player.controlsEnabled && dashPressed)
      Player.heat.DecreaseHeat(3);

    if (!Player.IsGrounded())
      return State.JUMPING;

    return null;
  }
}