using System;
using System.Collections;
using UnityEngine;

public class MainAudioController : MonoBehaviour
{
  [SerializeField] private AudioClip MenuBGM;
  [SerializeField] private AudioClip GameBGM;

  [NonSerialized] public AudioSource BGMSource;

  public void Awake()
  {
    BGMSource = GetComponent<AudioSource>();
  }

  public void PlayMenuBGM()
  {
    if (BGMSource.isPlaying)
      StartCoroutine(FadeOutFadeIn(BGMSource, MenuBGM));
    else
    {
      BGMSource.clip = MenuBGM;
      BGMSource.Play();
      StartCoroutine(FadeIn(BGMSource));
    }
  }

  public void PlayGameBGM()
  {
    if (BGMSource.isPlaying)
      StartCoroutine(FadeOutFadeIn(BGMSource, GameBGM));
    else
    {
      BGMSource.clip = GameBGM;
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
