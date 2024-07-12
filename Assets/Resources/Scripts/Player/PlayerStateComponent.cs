using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerStateComponent : MonoBehaviour
{
  private Player Player;

  public PlayerStandingState standingState { get; private set; }
  public PlayerHeatRecoveryState heatRecoveryState { get; private set; }
  public PlayerJumpingState jumpingState { get; private set; }
  public PlayerFallingState fallingState { get; private set; }
  public PlayerDashingState dashingState { get; private set; }
  public PlayerDeadState deadState { get; private set; }

  private Dictionary<State, PlayerState> states;

  public PlayerState currentState { get; private set; }
  public PlayerState previousState { get; private set; }
  public float lastTransitionTime { get; private set; } = 0f;

  private void Awake()
  {
    Player = GetComponent<Player>();
    standingState = new PlayerStandingState(Player);
    heatRecoveryState = new PlayerHeatRecoveryState(Player);
    jumpingState = new PlayerJumpingState(Player);
    fallingState = new PlayerFallingState(Player);
    dashingState = new PlayerDashingState(Player);
    deadState = new PlayerDeadState(Player);


    states = new Dictionary<State, PlayerState>() {
      {State.STANDING, standingState},
      {State.HEAT_RECOVERY, heatRecoveryState},
      {State.JUMPING, jumpingState},
      {State.FALLING, fallingState},
      {State.DASHING, dashingState},
      {State.DEAD, deadState},
    };

    currentState = fallingState;
    previousState = fallingState;
  }

  void Update()
  {
    State? newState = currentState.CustomUpdate();
    if (newState.HasValue)
      TransitionToState(newState.Value);
  }

  void FixedUpdate()
  {
    State? newState = currentState.CustomFixedUpdate();
    if (newState.HasValue)
      TransitionToState(newState.Value);
  }

  public void TransitionToState(State newState)
  {
    if (currentState.state == newState)
    {
      if (newState != State.DASHING)
        return;
      else if (newState == State.DASHING && !dashingState.canDash)
        return;
    }
    Debug.Log(currentState.state + " - " + newState + " | " + Time.time);

    currentState.Exit();
    previousState = currentState;
    lastTransitionTime = Time.time;
    currentState = states[newState];
    currentState.Enter();
  }
}
