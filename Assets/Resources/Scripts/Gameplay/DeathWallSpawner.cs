
using UnityEngine;
using UnityEngine.SceneManagement;

public enum DeathWallExpensionDirection
{
  LEFT,
  RIGHT,
  UP,
  DOWN,
}

public class DeathWallSpawner : MonoBehaviour
{
  [SerializeField] private GameObject DeathWallPrefab;
  [SerializeField] private DeathWallExpensionDirection expensionDirection;
  [SerializeField] private float expensionSpeed = 1;
  private int sortingOrder = System.Int16.MaxValue;

  private Vector2 lastSpawnedAt = Vector2.zero;

  void Start()
  {
    lastSpawnedAt = transform.position;
    switch (expensionDirection)
    {
      case DeathWallExpensionDirection.UP:
        transform.rotation = Quaternion.Euler(0, 0, 90);
        break;
      case DeathWallExpensionDirection.DOWN:
        transform.rotation = Quaternion.Euler(0, 0, -90);
        break;
      case DeathWallExpensionDirection.LEFT:
        transform.rotation = Quaternion.Euler(0, 0, 180);
        break;
      case DeathWallExpensionDirection.RIGHT:
      default:
        break;
    }
    InvokeRepeating("SpawnFireWall", 0, 1f / expensionSpeed);
  }

  void SpawnFireWall()
  {
    // FIXME: If spawning during async scene load, ends up in sceneManager and never get destroyed
    if (SceneManager.GetActiveScene().name == "SceneManager")
      return;
    Vector2 newPos = GetNewSpawnPosition();
    SpriteRenderer newWallSpriteRenderer = Instantiate(DeathWallPrefab, newPos, transform.rotation).GetComponent<SpriteRenderer>();
    newWallSpriteRenderer.sortingOrder = sortingOrder;
    sortingOrder--;
    lastSpawnedAt = newPos;
  }

  Vector2 GetNewSpawnPosition()
  {
    return expensionDirection switch
    {
      DeathWallExpensionDirection.UP => new Vector2(lastSpawnedAt.x + Random.Range(-2f, 2f), lastSpawnedAt.y + 0.8f),
      DeathWallExpensionDirection.DOWN => new Vector2(lastSpawnedAt.x + Random.Range(-2f, 2f), lastSpawnedAt.y - 0.8f),
      DeathWallExpensionDirection.LEFT => new Vector2(lastSpawnedAt.x - 0.8f, lastSpawnedAt.y + Random.Range(-2f, 2f)),
      DeathWallExpensionDirection.RIGHT => new Vector2(lastSpawnedAt.x + 0.8f, lastSpawnedAt.y + Random.Range(-2f, 2f)),
      _ => Vector2.zero,
    };
  }
}
