using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player))]
public class PlayerInputComponent : MonoBehaviour
{
  [NonSerialized] public PlayerInput playerInput;
  [NonSerialized] public InputActionAsset actions;

  public Vector2 movementVector = Vector2.zero;
  public bool controlsEnabled = true;

  public bool dashPressed = false, dashHeld = false, dashReleased = false;
  public bool jumpPressed = false, jumpHeld = false, jumpReleased = false;
  private Player Player;

  private void Awake()
  {
    playerInput = GetComponent<PlayerInput>();
  }

  private void Start()
  {
    actions = playerInput.actions;
  }

  void Update()
  {
    jumpPressed = actions["Jump"].WasPressedThisFrame();
    if (jumpPressed)
      jumpHeld = true;

    jumpReleased = actions["Jump"].WasReleasedThisFrame();
    if (jumpReleased)
      jumpHeld = false;

    dashPressed = actions["Dash"].WasPressedThisFrame();
    if (dashPressed)
      dashHeld = true;

    dashReleased = actions["Dash"].WasReleasedThisFrame();
    if (dashReleased)
      dashHeld = false;
  }

  public void HandleMovement(InputAction.CallbackContext context)
  {
    movementVector = context.ReadValue<Vector2>();
  }

  public bool JumpPressed() => controlsEnabled && jumpPressed;
  public bool JumpHeld() => controlsEnabled && jumpHeld;
  public bool JumpReleased() => controlsEnabled && jumpReleased;

  public bool DashPressed() => controlsEnabled && dashPressed;
  public bool DashHeld() => controlsEnabled && dashHeld;
  public bool DashReleased() => controlsEnabled && dashReleased;
}
