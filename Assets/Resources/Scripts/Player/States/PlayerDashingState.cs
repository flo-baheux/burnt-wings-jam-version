using System;
using System.Collections;
using UnityEngine;

public class PlayerDashingState : PlayerState
{
  private float dashTime = 0.15f;
  private float enterGracePeriod = 0.2f;
  private float exitGracePeriod = 0.1f;
  private bool isDashing = false;
  public bool canDash = false;
  float initialGravityScale = 0f;
  public Coroutine dashCoroutine = null;

  public PlayerDashingState(Player player) : base(player)
  {
    this.state = State.DASHING;
  }

  public Action OnDashMovementStart;

  public override void Enter()
  {
    canDash = false;
    isDashing = true;
    initialGravityScale = Player.rigidBody.gravityScale;
    dashCoroutine = Player.StartCoroutine(Dash());
    base.Enter();
  }

  public override State? CustomUpdate()
  {
    if (!isDashing)
      return Player.IsGrounded() ? State.STANDING : State.FALLING;

    return base.CustomUpdate();
  }

  public override void Exit()
  {
    Player.StopCoroutine(dashCoroutine);
    canDash = true;
    isDashing = false;
    Player.input.controlsEnabled = true;
    Player.rigidBody.gravityScale = initialGravityScale;
    base.Exit();
  }

  private IEnumerator Dash()
  {
    Player.heat.IncreaseHeat(Player.GetCurrentDashCost());
    Player.input.controlsEnabled = false;

    Player.rigidBody.gravityScale = 0f;
    Player.rigidBody.velocity *= 0.1f;

    Vector2 dashDirection = GetDashDirection();

    float timer = 0f;
    while (timer <= enterGracePeriod)
    {
      if (Player.input.movementVector.magnitude > 0.1f)
        dashDirection = GetDashDirection();
      timer += Time.deltaTime;
      yield return null;
    }

    OnDashMovementStart?.Invoke();
    Player.rigidBody.velocity = dashDirection * Player.DashStrength;
    yield return new WaitForSeconds(dashTime);

    Player.rigidBody.velocity *= 0.1f;
    Player.input.controlsEnabled = true;

    canDash = true;
    yield return new WaitForSeconds(exitGracePeriod);

    // Player.rigidBody.velocity = Vector2.zero;
    isDashing = false;
  }

  private Vector2 GetDashDirection()
    => new(Mathf.RoundToInt(Player.input.movementVector.normalized.x), Mathf.RoundToInt(Player.input.movementVector.normalized.y));
}