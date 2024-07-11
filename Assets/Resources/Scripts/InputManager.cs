using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public enum PlayerActionInputType
{
  PRESSED,
  RELEASED
}

public struct PlayerAction
{
  public PlayerAction(string actionName, PlayerActionInputType inputType, float pressedAt)
  {
    this.actionName = actionName;
    this.inputType = inputType;
    this.pressedAt = pressedAt;
  }

  public string actionName { get; private set; }
  public float pressedAt { get; private set; }
  public PlayerActionInputType inputType { get; private set; }
};

public class InputManager : MonoBehaviour
{
  [SerializeField] private float inputBufferTime;
  private List<PlayerAction> playerActions = new();
  public PlayerInput playerInput { get; private set; }

  public PlayerAction? LookUpPlayerAction() => playerActions.Count > 0 ? playerActions.Last() : null;
  public void ConsumePlayerAction(PlayerAction? input)
  {
    if (input.HasValue)
    {
      Debug.Log("CONSUMED " + input?.actionName + " " + input?.inputType);
      playerActions.Remove(input.Value);
    }
  }

  void Awake()
  {
    playerInput = GetComponent<PlayerInput>();
  }

  private void Update()
  {
    if (playerInput.actions["Jump"].WasPressedThisFrame())
    {
      Debug.Log("JUMP PRESSED " + Time.time);
      playerActions.Add(new PlayerAction("Jump", PlayerActionInputType.PRESSED, Time.time));
    }
    if (playerInput.actions["Jump"].WasReleasedThisFrame())
    {
      Debug.Log("JUMP RELEASED " + Time.time);
      playerActions.Add(new PlayerAction("Jump", PlayerActionInputType.RELEASED, Time.time));
    }
    if (playerInput.actions["Dash"].WasPressedThisFrame())
    {
      Debug.Log("DASH PRESSED " + Time.time);
      playerActions.Add(new PlayerAction("Dash", PlayerActionInputType.PRESSED, Time.time));
    }
    if (playerInput.actions["Dash"].WasReleasedThisFrame())
    {
      Debug.Log("DASH RELEASED " + Time.time);
      playerActions.Add(new PlayerAction("Dash", PlayerActionInputType.RELEASED, Time.time));
    }

    foreach (PlayerAction playerAction in playerActions.ToList())
    {

      if (!IsPlayerActionValid(playerAction))
      {
        Debug.Log("Removing input " + playerAction.actionName + " - " + playerAction.inputType);
        playerActions.Remove(playerAction);
      }
    }
  }

  private bool IsPlayerActionValid(PlayerAction playerAction) => Time.time <= playerAction.pressedAt + inputBufferTime;
}
