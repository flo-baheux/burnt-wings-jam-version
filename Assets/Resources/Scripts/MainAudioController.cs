using System;
using System.Collections;
using UnityEngine;

public class MainAudioController : MonoBehaviour
{
  [SerializeField] private AudioClip FirstBGM;
  [SerializeField] private AudioClip defaultAmbiantSounds;

  [SerializeField, Range(0, 1)] float maxBGMvolume;
  [SerializeField, Range(0, 1)] float maxSFXvolume;

  [NonSerialized] public AudioSource BGMSource;
  [NonSerialized] public AudioSource SFXSource;

  public void Awake()
  {
    AudioSource[] sources = GetComponents<AudioSource>();
    foreach (AudioSource source in sources)
    {
      if (source.outputAudioMixerGroup.name == "BGM")
        BGMSource = source;
      else if (source.outputAudioMixerGroup.name == "SFX")
        SFXSource = source;
    }
  }

  public void PlayDefaultAmbiantSounds()
  {
    SFXSource.clip = defaultAmbiantSounds;
    SFXSource.Play();
  }

  public void PlayFirstBGM()
  {
    if (BGMSource.isPlaying)
      StartCoroutine(FadeOutFadeIn(BGMSource, FirstBGM));
    else
    {
      BGMSource.clip = FirstBGM;
      BGMSource.Play();
      StartCoroutine(FadeIn(BGMSource));
    }
  }

  IEnumerator FadeOutFadeIn(AudioSource audioSource, AudioClip clip, int toVolume = 1, int fadeOutDuration = 1, int fadeInDuration = 1)
  {
    yield return StartCoroutine(FadeOut(audioSource, fadeOutDuration));
    audioSource.clip = clip;
    audioSource.Play();
    yield return StartCoroutine(FadeIn(audioSource, toVolume, fadeInDuration));
  }

  IEnumerator FadeOut(AudioSource audioSource, int duration = 1)
  {
    float timeElapsed = 0;

    while (audioSource.volume > 0)
    {
      audioSource.volume = Mathf.Lerp(1, 0, timeElapsed / duration);
      timeElapsed += Time.deltaTime;
      yield return null;
    }
  }

  IEnumerator FadeIn(AudioSource audioSource, int toVolume = 1, int duration = 1)
  {
    float timeElapsed = 0;

    int targetVolume = Math.Min(toVolume, 1);
    while (audioSource.volume < targetVolume)
    {
      audioSource.volume = Mathf.Lerp(0, 1, timeElapsed / duration);
      timeElapsed += Time.deltaTime;
      yield return null;
    }
  }
}
