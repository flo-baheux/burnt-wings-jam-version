using UnityEngine;

public class AudioToggle : MonoBehaviour
{
  public void ToggleAudio() =>
    AudioListener.volume = AudioListener.volume == 1 ? 0 : 1;
}
