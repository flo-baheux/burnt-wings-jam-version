using UnityEngine;

public class Checkpoint : MonoBehaviour
{
  private GameManager gameManager;

  void Awake()
  {
    gameManager = FindObjectOfType<GameManager>();
  }

  void OnTriggerEnter2D(Collider2D other)
  {
    if (other.TryGetComponent<Player>(out Player player))
      gameManager.SetLatestCheckpoint(this);
  }
}
