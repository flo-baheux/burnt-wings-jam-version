using System;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerDisplayComponent : MonoBehaviour
{
  [SerializeField] private ParticleSystem heatParticleSystem;
  [SerializeField] private ParticleSystem coolOffParticleSystem;
  [SerializeField] private ParticleSystem feathersParticleSystem;
  [SerializeField] private TrailRenderer dashTrailRenderer;
  [SerializeField] private float dashScreenshakeStrength = 0.2f;

  private bool facingRight = true;
  private Animator animator;
  private SpriteRenderer spriteRenderer;
  private CinemachineImpulseSource cameraImpulseSource;

  private ParticleSystem.EmissionModule heatParticleEmissionModule;
  private ParticleSystem.EmissionModule coolOffParticleEmissionModule;
  private ParticleSystem.EmissionModule feathersParticleEmissionModule;

  private Player Player;

  private void Awake()
  {
    Player = GetComponent<Player>();
    animator = GetComponent<Animator>();
    spriteRenderer = GetComponent<SpriteRenderer>();
    cameraImpulseSource = GetComponent<CinemachineImpulseSource>();

    heatParticleEmissionModule = heatParticleSystem.emission;
    coolOffParticleEmissionModule = coolOffParticleSystem.emission;
    feathersParticleEmissionModule = feathersParticleSystem.emission;

    heatParticleEmissionModule.enabled = false;
    coolOffParticleEmissionModule.enabled = false;
    feathersParticleEmissionModule.enabled = false;
    dashTrailRenderer.emitting = false;
  }

  private void Start()
  {
    Player.state.dashingState.OnEnter += HandleEnterDashingState;
    Player.state.dashingState.OnDashMovementStart += HandleDashMovementStart;
    Player.state.dashingState.OnExit += HandleExitDashingState;
  }

  void Update()
  {
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

    coolOffParticleEmissionModule.enabled = Player.state.currentState.state == State.HEAT_RECOVERY;

    heatParticleEmissionModule.enabled = Player.heat.currentHeat != 0;
    if (Player.heat.currentHeat < Player.heat.burnoutRecoveryThreshold)
      heatParticleEmissionModule.rateOverTime = 10;
    else if (!Player.heat.burnoutMode)
      heatParticleEmissionModule.rateOverTime = 20;
    else
      heatParticleEmissionModule.rateOverTime = 50;

  }

  private void HandleEnterDashingState(Player p) => animator.SetTrigger("Dash");
  private void HandleDashMovementStart()
  {
    cameraImpulseSource.GenerateImpulse(dashScreenshakeStrength);
    feathersParticleEmissionModule.enabled = true;
    // feathersParticleSystem.Emit(5);
    dashTrailRenderer.emitting = true;
  }
  private void HandleExitDashingState(Player p)
  {
    animator.ResetTrigger("Dash");
    feathersParticleEmissionModule.enabled = false;
    dashTrailRenderer.emitting = false;
  }
}
