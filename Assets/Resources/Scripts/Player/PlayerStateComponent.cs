using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerStateComponent : MonoBehaviour
{
  private Player Player;

  public PlayerGroundedState groundedState { get; private set; }
  public PlayerHeatRecoveryState heatRecoveryState { get; private set; }
  public PlayerJumpingState jumpingState { get; private set; }
  public PlayerDashingState dashingState { get; private set; }
  public PlayerDeadState deadState { get; private set; }

  private Dictionary<State, PlayerState> states;

  public PlayerState currentState;

  private void Awake()
  {
    Player = GetComponent<Player>();
    groundedState = new PlayerGroundedState(Player);
    heatRecoveryState = new PlayerHeatRecoveryState(Player);
    jumpingState = new PlayerJumpingState(Player);
    dashingState = new PlayerDashingState(Player);
    deadState = new PlayerDeadState(Player);


    states = new Dictionary<State, PlayerState>() {
      {State.GROUNDED, groundedState},
      {State.HEAT_RECOVERY, heatRecoveryState},
      {State.JUMPING, jumpingState},
      {State.DASHING, dashingState},
      {State.DEAD, deadState},
    };

    currentState = jumpingState;
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
    if (currentState.state == newState && newState != State.DASHING)
      return;
    currentState.Exit();
    currentState = states[newState];
    currentState.Enter();
  }
}
