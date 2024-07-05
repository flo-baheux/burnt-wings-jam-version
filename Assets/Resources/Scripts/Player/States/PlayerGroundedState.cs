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
    // bool movementPressed = Player.playerInput.actions["Move"].ReadValue<float>() != 0;

    // // Idle => freeze pos X to avoid sliding on slopes
    // if (!jumpPressed && !movementPressed && Player.IsGrounded())
    //   Player.rigidBody.constraints |= RigidbodyConstraints2D.FreezePositionX;
    // else
    //   Player.rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;

    if (Player.controlsEnabled && jumpPressed)
    {
      float jumpForce = Mathf.Sqrt(Player.JumpHeight * -2 * (Physics2D.gravity.y * Player.rigidBody.gravityScale));
      Player.rigidBody.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
      return State.JUMPING;
    }

    if (!Player.IsGrounded())
      return State.JUMPING;

    return null;
  }
}