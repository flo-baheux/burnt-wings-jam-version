using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
  // Movements
  [SerializeField] private float jumpHeight = 4f;
  public float JumpHeight => jumpHeight;
  [SerializeField] private float fallSpeed = 8f;
  public float FallSpeed => fallSpeed;
  [SerializeField] private float runningSpeed = 12f;
  public float RunningSpeed => runningSpeed;
  [SerializeField] private float dashStrength = 40f;
  public float DashStrength => dashStrength;

  public float overheatRunningSpeed = 20f;


  public Vector2 movementVector = Vector2.zero;

  private bool facingRight = true;
  public bool controlsEnabled = true;

  // Components
  [NonSerialized] public Rigidbody2D rigidBody;
  [NonSerialized] public PlayerInput playerInput;
  public PlayerStateComponent state { get; private set; }
  public PlayerHeatComponent heat { get; private set; }
  private CapsuleCollider2D mainCollider;
  public Animator animator;
  private SpriteRenderer spriteRenderer;

  // Gameplay Manager
  private GameManager gameplayManager;

  void Awake()
  {
    state = GetComponent<PlayerStateComponent>();
    heat = GetComponent<PlayerHeatComponent>();
    rigidBody = GetComponent<Rigidbody2D>();
    mainCollider = GetComponent<CapsuleCollider2D>();
    playerInput = GetComponent<PlayerInput>();
    animator = GetComponent<Animator>();
    spriteRenderer = GetComponent<SpriteRenderer>();
    gameplayManager = GameObject.Find("GameManager").GetComponent<GameManager>();
  }

  void Start()
  {
    heat.burnoutTriggered += () => state.TransitionToState(State.DEAD);
  }

  void Update()
  {

    if (controlsEnabled)
    {
      movementVector = playerInput.actions["Move"].ReadValue<Vector2>();
      if (movementVector.x > 0)
        facingRight = true;
      else if (movementVector.x < 0)
        facingRight = false;
    }

    spriteRenderer.flipX = !facingRight;
    animator.SetFloat("xAbsInput", Math.Abs(movementVector.x));
    animator.SetFloat("yVelocity", rigidBody.velocity.y);
    animator.SetBool("IsGrounded", state.currentState.state == State.GROUNDED);
    // animator.SetBool("IsDead", state.currentState.state == State.DEAD);
    if (playerInput.actions["Pause"].WasPressedThisFrame())
      gameplayManager.PauseResumeGame();

    // use facingRight to rotate character sprite to face left/right
    if (playerInput.actions["dash"].WasPressedThisFrame())
    {
      // If not moving and mashing, decrease heat.
      if (movementVector != Vector2.zero && !heat.overheatMode)
        state.TransitionToState(State.DASHING);
    }
  }

  void FixedUpdate()
  {
    if (controlsEnabled)
    {
      float horizontalVelocity = movementVector.x * (heat.overheatMode ? overheatRunningSpeed : runningSpeed);
      rigidBody.velocity = new Vector2(horizontalVelocity, rigidBody.velocity.y);
    }
  }

  public bool IsGrounded()
  {
    Vector2 center = new(mainCollider.bounds.center.x, mainCollider.bounds.min.y);
    Vector2 size = new(mainCollider.bounds.size.x, 0.05f);
    RaycastHit2D raycastHit = Physics2D.BoxCast(center, size, 0f, Vector2.down, 0f, LayerMask.GetMask("Ground"));
    return raycastHit.collider;
  }

  public void RespawnToPosition(Vector2 position)
  {
    rigidBody.position = position;
    state.TransitionToState(State.JUMPING);
    controlsEnabled = true;
  }

  public void OnTriggerEnter2D(Collider2D other)
  {
    if (other.CompareTag("Threat"))
      state.TransitionToState(State.DEAD);
    if (other.CompareTag("LevelEnd"))
      gameplayManager.StartNextLevel();
  }

  public int GetCurrentDashCost() => gameplayManager.dashHeatCost;
}
