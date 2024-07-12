using UnityEngine;

public class PlayerJumpingState : PlayerState
{
  private bool jump = false;
  private bool cancelJump = false;

  public PlayerJumpingState(Player player) : base(player)
  {
    this.state = State.JUMPING;
  }

  public override void Enter()
  {
    jump = true;
    cancelJump = false;
    base.Enter();
  }

  public override State? CustomUpdate()
  {
    if (Player.IsGrounded())
      return State.STANDING;

    if (Player.input.JumpReleased || Player.rigidBody.velocity.y <= 0f)
      return State.FALLING;

    return base.CustomUpdate();
  }

  public override State? CustomFixedUpdate()
  {
    if (jump)
    {
      jump = false;
      float jumpForce = Mathf.Sqrt(Player.JumpHeight * -2 * (Physics2D.gravity.y * Player.rigidBody.gravityScale));
      // Using AddForce (applying force once) is fine in update method - don't move it out
      Player.rigidBody.velocity = new Vector2(Player.rigidBody.velocity.x, 0);
      Player.rigidBody.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
    }

    if (cancelJump)
    {
      cancelJump = false;
      Player.rigidBody.velocity = new Vector2(Player.rigidBody.velocity.x, 0);
    }

    Player.rigidBody.velocity = new Vector2(Player.rigidBody.velocity.x, Mathf.Max(Player.rigidBody.velocity.y, -Player.FallSpeed));
    return base.CustomFixedUpdate();
  }

  // private IEnumerator BufferJump()
  // {
  //   jumpBuffered = true;
  //   yield return new WaitForSeconds(Player.JumpBuffer);
  //   jumpBuffered = false;
  // }
}