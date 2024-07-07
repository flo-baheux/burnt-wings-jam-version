using System.Collections;
using UnityEngine;

enum TimerState
{
  Off,
  Active,
  Collapsed,
}
public class HeatCollapsiblePlatform : MonoBehaviour
{
  [SerializeField] private float ActiveDuration = 2f;
  [SerializeField] private float CollapsedDuration = 5f;
  [SerializeField] private float FadeOutDuration = 1f;
  [SerializeField] private float FadeInDuration = 0.5f;
  private float targetTime = 0f;
  private TimerState state = TimerState.Off;
  private Collider2D Collider;
  private SpriteRenderer spriteRenderer;
  private Transform visual;

  [SerializeField] private Sprite platform1;
  [SerializeField] private Sprite platform2;
  [SerializeField] private Sprite platform3;

  // Start is called before the first frame update
  private void Awake()
  {
    state = TimerState.Off;
    targetTime = 0f;
    Collider = GetComponent<Collider2D>();

    visual = transform.GetChild(0);
    spriteRenderer = visual.gameObject.GetComponent<SpriteRenderer>();
    spriteRenderer.sprite = platform1;
  }

  // Update is called once per frame
  private void Update()
  {
    switch (state)
    {
      case TimerState.Active:
        spriteRenderer.sprite = platform2;
        targetTime -= Time.deltaTime;
        visual.localPosition = new Vector2(Mathf.PingPong(Time.time * 10f, 1f), visual.localPosition.y);
        if (targetTime <= 0.0f)
        {
          spriteRenderer.sprite = platform3;
          Collider.enabled = false;
          StartCoroutine(LerpTransparency(0, FadeOutDuration));
          targetTime = CollapsedDuration;
          state = TimerState.Collapsed;
        }
        break;

      case TimerState.Collapsed:
        targetTime -= Time.deltaTime;

        if (targetTime <= 0.0f)
        {
          StartCoroutine(LerpTransparency(1, FadeInDuration));

          targetTime = 0;
          spriteRenderer.sprite = platform1;
          state = TimerState.Off;
          Collider.enabled = true;
        }
        break;

      case TimerState.Off:
        visual.localPosition = Vector2.zero;
        break;
      default:
        break;
    }
  }

  void OnCollisionEnter2D(Collision2D other)
  {
    other.gameObject.TryGetComponent<Player>(out Player player);

    if (player && player.heat.IsBeyondHeatThreshold() && state == TimerState.Off)
    {
      targetTime = ActiveDuration;
      state = TimerState.Active;
    }
  }

  private IEnumerator LerpTransparency(float targetAlpha, float duration)
  {
    float timer = 0;
    Color startValue = spriteRenderer.color;
    Color endValue = spriteRenderer.color;
    endValue.a = targetAlpha;

    while (timer < duration)
    {
      spriteRenderer.color = Color.Lerp(startValue, endValue, timer / duration);
      timer += Time.deltaTime;
      yield return null;
    }
    spriteRenderer.color = endValue;
  }
}
