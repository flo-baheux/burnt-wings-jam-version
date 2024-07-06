using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LoadLevelButton : MonoBehaviour
{
  [SerializeField] private int world;
  [SerializeField] private int level;

  private Button button;
  private GameManager gameManager;


  void Awake()
  {
    button = GetComponent<Button>();
    gameManager = FindObjectOfType<GameManager>();
  }

  void Start()
  {
    button.onClick.AddListener(() => gameManager.StartLevel(world, level));
  }

}
