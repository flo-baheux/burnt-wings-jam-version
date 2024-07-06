using UnityEngine;

public class PlayerDashingState : PlayerState
{
  private float dashTime = 0.15f;
  private float initialGravityScale = 0f;
  private float dashTimer = 0f;
  private float outOfDashGracePeriod = 0.1f;
  private bool isDashing = false, isGracePeriod = false;

  public PlayerDashingState(Player player) : base(player)
  {
    this.state = State.DASHING;
  }

  public override void Enter()
  {
    base.Enter();

    isDashing = true;
    isGracePeriod = false;
    Player.heat.IncreaseHeat();
    initialGravityScale = Player.rigidBody.gravityScale;
    Player.rigidBody.gravityScale = 0f;
    Player.controlsEnabled = false;
    Player.rigidBody.velocity = Player.movementVector * Player.DashStrength;
  }


  // FIXME: That would be a lot easier with a coroutine...
  public override State? CustomUpdate()
  {
    // collision = early stop?


    if (isDashing && dashTimer <= dashTime)
    {
      dashTimer += Time.deltaTime;
      return base.CustomUpdate();
    }

    if (isDashing)
    {
      isDashing = false;
      dashTimer = 0f;
      isGracePeriod = true;
      Player.controlsEnabled = true;
      Player.rigidBody.velocity = Vector2.zero;
    }

    if (isGracePeriod && dashTimer <= outOfDashGracePeriod)
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
    Player.controlsEnabled = true;
    Player.rigidBody.velocity = Vector2.zero;
    Player.rigidBody.gravityScale = initialGravityScale;
  }
}