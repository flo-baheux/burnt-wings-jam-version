using System;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerDisplayComponent : MonoBehaviour
{
  [SerializeField] private ParticleSystem.EmissionModule particleEmission;
  [SerializeField] private GameObject coolOffParticles;
  [SerializeField] private float dashScreenshakeStrength = 0.2f;

  private bool facingRight = true;
  private Animator animator;
  private SpriteRenderer spriteRenderer;
  private CinemachineImpulseSource impulseSource;

  private TrailRenderer trailRenderer;

  private Player Player;

  private void Awake()
  {
    Player = GetComponent<Player>();
    animator = GetComponent<Animator>();
    spriteRenderer = GetComponent<SpriteRenderer>();
    impulseSource = GetComponent<CinemachineImpulseSource>();
    particleEmission = GetComponent<ParticleSystem>().emission;
    trailRenderer = GetComponentInChildren<TrailRenderer>();
    coolOffParticles.SetActive(false);
  }

  private void Start()
  {
    Player.state.dashingState.OnEnter += HandleEnterDashingState;
    Player.state.dashingState.OnDashMovementStart += HandleDashMovementStart;
    Player.state.dashingState.OnExit += HandleExitDashingState;
  }

  void Update()
  {
    trailRenderer.material.SetColor("EmissionColor", Player.input.JumpHeld ? Color.red : Color.white);
    if (Player.input.movementVector.x > 0)
      facingRight = true;
    else if (Player.input.movementVector.x < 0)
      facingRight = false;
    spriteRenderer.flipX = !facingRight;

    animator.SetFloat("xAbsInput", Math.Abs(Player.input.movementVector.x));
    animator.SetFloat("yVelocity", Player.rigidBody.velocity.y);
    animator.SetBool("IsGrounded", Player.state.currentState.state == State.STANDING);
    animator.SetBool("IsCoolingOff", Player.state.currentState.state == State.HEAT_RECOVERY);
    animator.SetBool("Overheat", Player.heat.IsBeyondHeatThreshold());
    coolOffParticles.SetActive(Player.state.currentState.state == State.HEAT_RECOVERY);

    particleEmission.enabled = Player.heat.currentHeat != 0;
    if (Player.heat.currentHeat < Player.heat.burnoutRecoveryThreshold)
      particleEmission.rateOverTime = 10;
    else if (!Player.heat.burnoutMode)
      particleEmission.rateOverTime = 20;
    else
      particleEmission.rateOverTime = 50;

  }

  private void HandleEnterDashingState(Player p) => animator.SetTrigger("Dash");
  private void HandleDashMovementStart() => impulseSource.GenerateImpulse(dashScreenshakeStrength);
  private void HandleExitDashingState(Player p) => animator.ResetTrigger("Dash");
}
