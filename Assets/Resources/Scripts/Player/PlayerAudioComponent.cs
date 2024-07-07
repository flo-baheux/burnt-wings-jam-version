using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerAudioComponent : MonoBehaviour
{
  public AudioClip burnoutSound;
  public AudioClip coolOffSound;
  public List<AudioClip> dashSounds;
  public List<AudioClip> deathSounds;
  private AudioSource SFXAudioSource;
  private Player player;

  void Awake()
  {
    SFXAudioSource = GetComponent<AudioSource>();
    player = GetComponent<Player>();
  }

  void Start()
  {
    player.heat.BurnoutTriggered += HandleBurnout;
    player.state.dashingState.OnEnter += HandleDash;
    player.state.deadState.OnEnter += HandleDash;
    player.state.heatRecoveryState.OnEnter += HandleCoolOffStart;
    player.state.heatRecoveryState.OnExit += HandleCoolOffStop;
  }

  public void HandleBurnout() => SFXAudioSource.PlayOneShot(burnoutSound);
  public void HandleDash(Player p) => PlayDashSound();
  public void HandleDeath(Player p) => PlayDeathSound();
  public void HandleCoolOffStart(Player p) => PlayCoolOffSound();
  public void HandleCoolOffStop(Player p) => StopCoolOffSound();

  public void PlayDeathSound() => SFXAudioSource.PlayOneShot(deathSounds.OrderBy(n => Guid.NewGuid()).ToArray()[0]);
  public void PlayDashSound() => SFXAudioSource.PlayOneShot(dashSounds.OrderBy(n => Guid.NewGuid()).ToArray()[0]);

  public void PlayCoolOffSound()
  {
    SFXAudioSource.clip = coolOffSound;
    SFXAudioSource.Play();
  }

  public void StopCoolOffSound() => SFXAudioSource.Stop();
}

