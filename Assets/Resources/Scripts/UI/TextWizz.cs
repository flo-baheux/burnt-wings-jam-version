using UnityEngine;

public class TextWizz : MonoBehaviour
{
  public float speed = 1f;
  public int amplitude = 10;
  private Vector2 initialPosition;

  // Start is called before the first frame update
  void Start()
  {
    initialPosition = transform.position;
  }

  // Update is called once per frame
  void Update()
  {
    transform.position = new Vector2(initialPosition.x + Mathf.PingPong(Time.time * speed, 10) - amplitude / 2, initialPosition.y);
  }
}
