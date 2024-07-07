using UnityEngine;

public class PlayerDashingState : PlayerState
{
  private float dashTime = 0.15f;
  private float initialGravityScale = 0f;
  private float dashTimer = 0f;
  private float enterGracePeriod = 0.20f;
  private bool isDashing = false, isGracePeriod = false;

  public PlayerDashingState(Player player) : base(player)
  {
    this.state = State.DASHING;
  }

  public override void Enter()
  {
    base.Enter();

    dashTimer = 0f;
    isDashing = true;
    isGracePeriod = true;
    Player.animator.SetTrigger("Dash");
    Player.heat.IncreaseHeat(Player.GetCurrentDashCost());
    initialGravityScale = Player.rigidBody.gravityScale;
    Player.rigidBody.gravityScale = 0f;
    Player.controlsEnabled = false;
    Player.rigidBody.velocity *= 0.1f;

  }


  // FIXME: That would be a lot easier with a coroutine...
  public override State? CustomUpdate()
  {
    // collision = early stop?
    if (isGracePeriod && dashTimer <= enterGracePeriod)
    {
      dashTimer += Time.deltaTime;
      return base.CustomUpdate();
    }

    if (isGracePeriod)
    {
      isGracePeriod = false;
      dashTimer = 0f;
      Player.rigidBody.velocity = Player.movementVector * Player.DashStrength;
    }

    if (isDashing && dashTimer <= dashTime)
    {
      dashTimer += Time.deltaTime;
      return base.CustomUpdate();
    }

    // if (isDashing)
    // {
    //   isDashing = false;
    //   dashTimer = 0f;
    //   isGracePeriod = true;
    //   Player.controlsEnabled = true;
    //   Player.rigidBody.velocity = Vector2.zero;
    // }

    // if (isGracePeriod && dashTimer <= exitGracePeriod)
    // {
    //   dashTimer += Time.deltaTime;
    //   return base.CustomUpdate();
    // }

    return State.JUMPING;
  }

  public override void Exit()
  {
    base.Exit();

    Player.rigidBody.velocity = Vector2.zero;
    Player.controlsEnabled = true;
    Player.rigidBody.gravityScale = initialGravityScale;
    Player.animator.ResetTrigger("Dash");

  }
}