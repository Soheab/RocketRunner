using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float accel = 35f;
    [SerializeField] private float decel = 45f;
    [SerializeField] private float linearDamping = 3f;
    [SerializeField] private float everySeconds = 10f;
    [SerializeField] private Sprite rocketSprite;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private float currentMaxSpeed;
    private float difficultyTimer;

    private void Reset()
    {
        rocketSprite = null;
    }

    private void Awake()
    {
        EnsureRocketVisual();

        rb = GetComponent<Rigidbody2D>();
        rb.linearDamping = Mathf.Clamp(linearDamping, 2f, 4f);
        rb.constraints |= RigidbodyConstraints2D.FreezeRotation;
        currentMaxSpeed = SettingsData.PlayerSpeed;
    }

    private void EnsureRocketVisual()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();

        if (rocketSprite == null)
        {
            rocketSprite = Resources.Load<Sprite>("Sprites/RocketSprite");
            if (rocketSprite == null)
            {
                Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/RocketSprite");
                if (sprites != null && sprites.Length > 0)
                    rocketSprite = sprites[0];
            }
        }

        if (rocketSprite != null)
            spriteRenderer.sprite = rocketSprite;
    }

    private void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.Ended) return;

        difficultyTimer += Time.deltaTime;
        while (difficultyTimer >= everySeconds)
        {
            difficultyTimer -= everySeconds;
            float maxSpeedCap = SettingsData.PlayerSpeed + 4f;
            currentMaxSpeed = Mathf.Min(currentMaxSpeed + SettingsData.PlayerSpeedStepPer10s, maxSpeedCap);
        }
    }

    private void FixedUpdate()
    {
        Vector2 targetVel = moveInput * currentMaxSpeed;

        Vector2 delta = targetVel - rb.linearVelocity;
        float rate = (moveInput.sqrMagnitude > 0.001f) ? accel : decel;

        Vector2 change = Vector2.ClampMagnitude(delta, rate * Time.fixedDeltaTime);
        rb.linearVelocity += change;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        if (moveInput.sqrMagnitude > 1f) moveInput = moveInput.normalized;
    }
}
