using UnityEngine;

public class PlayerDashingState : PlayerState
{
  private float dashTime = 0.15f;
  private float initialGravityScale = 0f;
  private float dashTimer = 0f;

  public PlayerDashingState(Player player) : base(player)
  {
    this.state = State.DASHING;
  }

  public override void Enter()
  {
    base.Enter();

    Player.dashesLeft--;
    initialGravityScale = Player.rigidBody.gravityScale;
    Player.rigidBody.gravityScale = 0f;
    Player.controlsEnabled = false;
    Player.rigidBody.velocity = Player.movementVector * Player.DashStrength;
  }

  public override State? CustomUpdate()
  {
    // collision = early stop?

    if (dashTimer <= dashTime)
    {
      dashTimer += Time.deltaTime;
      return base.CustomUpdate();
    }

    return State.JUMPING;
  }

  public override void Exit()
  {
    base.Exit();

    dashTimer = 0f;
    Player.rigidBody.gravityScale = initialGravityScale;
    Player.rigidBody.velocity = Vector2.zero;
    Player.controlsEnabled = true;
  }
}