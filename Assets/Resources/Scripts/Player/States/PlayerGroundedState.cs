using UnityEngine;

public class PlayerGroundedState : PlayerState
{
  private float currentY;

  public PlayerGroundedState(Player player) : base(player)
  {
    this.state = State.GROUNDED;
  }

  public override State? CustomUpdate()
  {
    bool jumpPressed = Player.playerInput.actions["Jump"].WasPressedThisFrame();

    if (Player.controlsEnabled && jumpPressed)
    {
      float jumpForce = Mathf.Sqrt(Player.JumpHeight * -2 * (Physics2D.gravity.y * Player.rigidBody.gravityScale));
      // Using AddForce (applying force once) is fine in update method - don't move it out
      Player.rigidBody.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
      return State.JUMPING;
    }

    if (!Player.IsGrounded())
      return State.JUMPING;

    return null;
  }
}