using UnityEngine;
using UnityEngine.UI;

public class AudioToggle : MonoBehaviour
{
  [SerializeField] private Sprite toggleOn;
  [SerializeField] private Sprite toggleOff;
  private Image image;

  void Awake()
  {
    image = GetComponent<Image>();
  }

  public void ToggleAudio()
  {
    AudioListener.volume = AudioListener.volume == 1 ? 0 : 1;
    image.sprite = AudioListener.volume == 1 ? toggleOff : toggleOn;
  }

}
