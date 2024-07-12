using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player)), RequireComponent(typeof(PlayerInput))]
public class PlayerInputComponent : MonoBehaviour
{
  [NonSerialized] public PlayerInput playerInput;
  [NonSerialized] public InputActionAsset actions;

  [SerializeField] private float jumpBuffer = 0.075f;
  public float JumpBuffer => jumpBuffer;

  public Vector2 movementVector = Vector2.zero;
  public bool controlsEnabled = true;

  private bool dashPressed = false, dashHeld = false, dashReleased = false;
  private bool jumpPressed = false, jumpHeld = false, jumpReleased = false, jumpBuffered = false;

  public bool JumpBuffered => controlsEnabled && jumpBuffered;
  public bool JumpPressed => controlsEnabled && jumpPressed;
  public bool JumpHeld => controlsEnabled && jumpHeld;
  public bool JumpReleased => controlsEnabled && jumpReleased;

  public bool DashPressed => controlsEnabled && dashPressed;
  public bool DashHeld => controlsEnabled && dashHeld;
  public bool DashReleased => controlsEnabled && dashReleased;


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
    {
      StartCoroutine(BufferJump());
      jumpHeld = true;
    }

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

  private IEnumerator BufferJump()
  {
    jumpBuffered = true;
    yield return new WaitForSeconds(JumpBuffer);
    jumpBuffered = false;
  }
}
