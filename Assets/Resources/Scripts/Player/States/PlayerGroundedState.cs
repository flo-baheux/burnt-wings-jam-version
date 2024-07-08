using System.Collections;
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
      Player.rigidBody.velocity = new Vector2(Player.rigidBody.velocity.x, 0);
      Player.rigidBody.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
      return State.JUMPING;
    }

    if (Player.controlsEnabled && dashPressed && Player.movementVector.magnitude <= 0.1f)
      return State.HEAT_RECOVERY;

    if (!Player.IsGrounded())
      return State.JUMPING;

    return null;
  }

  public override void Exit()
  {
    Player.StartCoroutine(ActivateCoyoteTime());
    base.Exit();
  }

  public IEnumerator ActivateCoyoteTime()
  {
    Player.wasRecentlyGrounded = true;
    yield return new WaitForSeconds(Player.coyoteTime);
    Player.wasRecentlyGrounded = false;
  }
}