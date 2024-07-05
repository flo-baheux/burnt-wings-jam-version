using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerStateComponent : MonoBehaviour
{
  private Player Player;

  public PlayerGroundedState groundedState { get; private set; }
  public PlayerJumpingState jumpingState { get; private set; }
  public PlayerDeadState deadState { get; private set; }

  private Dictionary<State, PlayerState> states;

  public PlayerState currentState;

  private void Awake()
  {
    Player = GetComponent<Player>();
    groundedState = new PlayerGroundedState(Player);
    jumpingState = new PlayerJumpingState(Player);
    deadState = new PlayerDeadState(Player);

    states = new Dictionary<State, PlayerState>() {
      {State.GROUNDED, groundedState},
      {State.JUMPING, jumpingState},
      {State.DEAD, deadState},
    };

    currentState = jumpingState;
  }

  public void Update()
  {
    State? newState = currentState.CustomUpdate();
    if (newState.HasValue)
      TransitionToState(newState.Value);
  }

  public void TransitionToState(State newState)
  {
    currentState.Exit();
    currentState = states[newState];
    currentState.Enter();
  }
}
