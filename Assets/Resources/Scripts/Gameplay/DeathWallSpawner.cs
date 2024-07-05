
using UnityEngine;

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
  [SerializeField] private int expensionSpeed = 1;

  private Vector2 lastSpawnedAt = Vector2.zero;
  // We gonna spawn many ones as the wall expands.
  // Assumption: the sprite itself is large enough, we don't bother with sprite

  // We spawn one every x sec in the expensionDirection direction.
  // it has to be slightly shifted sideway (going right -> few pixels top/bottom)
  // Don't overdo it as it's not the final sprite + no anim yet.

  // SOUND
  // There is ONE fire sound emitter that has to be moved where the last one is spawned (?)

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
    InvokeRepeating("SpawnFireWall", 0, 1 / expensionSpeed);
  }

  void SpawnFireWall()
  {
    Vector2 newPos = GetNewSpawnPosition();
    Instantiate(DeathWallPrefab, newPos, transform.rotation);
    lastSpawnedAt = newPos;
  }

  Vector2 GetNewSpawnPosition()
  {
    return expensionDirection switch
    {
      DeathWallExpensionDirection.UP => new Vector2(lastSpawnedAt.x + Random.Range(-1f, 1f), lastSpawnedAt.y + 1),
      DeathWallExpensionDirection.DOWN => new Vector2(lastSpawnedAt.x + Random.Range(-1f, 1f), lastSpawnedAt.y - 1),
      DeathWallExpensionDirection.LEFT => new Vector2(lastSpawnedAt.x - 1, lastSpawnedAt.y + Random.Range(-1f, 1f)),
      DeathWallExpensionDirection.RIGHT => new Vector2(lastSpawnedAt.x + 1, lastSpawnedAt.y + Random.Range(-1f, 1f)),
      _ => Vector2.zero,
    };
  }
}
