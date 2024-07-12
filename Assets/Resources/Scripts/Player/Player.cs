using System;
using UnityEngine;

public class Player : MonoBehaviour
{
  [Header("Movements")]
  [SerializeField] private float jumpHeight = 4f;
  public float JumpHeight => jumpHeight;
  [SerializeField] private float fallSpeed = 8f;
  public float FallSpeed => fallSpeed;
  [SerializeField] private float runningSpeed = 12f;
  public float RunningSpeed => runningSpeed;
  [SerializeField] private float dashStrength = 40f;
  public float DashStrength => dashStrength;

  public float coyoteTime = 0.1f;

  [Header("Burnout")]
  public float burnoutRunningSpeed = 20f;

  // Components
  [NonSerialized] public Rigidbody2D rigidBody;
  public PlayerStateComponent state { get; private set; }
  public PlayerHeatComponent heat { get; private set; }
  public PlayerInputComponent input { get; private set; }
  private Collider2D mainCollider;


  // Gameplay Manager
  private GameManager gameplayManager;

  void Awake()
  {
    state = GetComponent<PlayerStateComponent>();
    heat = GetComponent<PlayerHeatComponent>();
    input = GetComponent<PlayerInputComponent>();
    rigidBody = GetComponent<Rigidbody2D>();
    mainCollider = GetComponent<Collider2D>();
    gameplayManager = GameObject.Find("GameManager").GetComponent<GameManager>();
  }

  void Start()
  {
    heat.BurnoutOver += () => state.TransitionToState(State.DEAD);
  }

  void Update()
  {
    if (input.DashPressed && input.movementVector.magnitude >= 0.1f && !heat.burnoutMode)
      state.TransitionToState(State.DASHING);
  }

  void FixedUpdate()
  {
    if (input.controlsEnabled)
    {
      float horizontalVelocity = input.movementVector.x * (heat.burnoutMode ? burnoutRunningSpeed : runningSpeed);
      rigidBody.velocity = new Vector2(horizontalVelocity, rigidBody.velocity.y);
    }
  }

  public bool IsGrounded()
  {
    Vector2 center = new(mainCollider.bounds.center.x, mainCollider.bounds.min.y);
    Vector2 size = new(mainCollider.bounds.size.x, 0.1f);
    RaycastHit2D raycastHit = Physics2D.BoxCast(center, size, 0f, Vector2.down, 0f, LayerMask.GetMask("Ground"));
    return raycastHit.collider;
  }

  public void RespawnToPosition(Vector2 position)
  {
    rigidBody.position = position;
    state.TransitionToState(State.FALLING);
    input.controlsEnabled = true;
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
