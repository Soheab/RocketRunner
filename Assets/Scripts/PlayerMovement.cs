using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float accel = 35f;
    [SerializeField] private float decel = 45f;
    [SerializeField] private float linearDamping = 3f;
    [SerializeField] private float everySeconds = 10f;

    private Camera mainCam;
    private float halfWidth;
    private float halfHeight;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private float currentMaxSpeed;
    private float difficultyTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearDamping = Mathf.Clamp(linearDamping, 2f, 4f);
        rb.constraints |= RigidbodyConstraints2D.FreezeRotation;
        currentMaxSpeed = SettingsData.PlayerSpeed;

        rb = GetComponent<Rigidbody2D>();
        mainCam = Camera.main;

        float vertExtent = mainCam.orthographicSize;
        float horExtent = vertExtent * mainCam.aspect;

        halfWidth = horExtent;
        halfHeight = vertExtent;
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

        Vector3 pos = transform.position;

        pos.x = Mathf.Clamp(pos.x, -halfWidth + 0.5f, halfWidth - 0.5f);
        pos.y = Mathf.Clamp(pos.y, -halfHeight + 0.5f, halfHeight - 0.5f);

        transform.position = pos;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        if (moveInput.sqrMagnitude > 1f) moveInput = moveInput.normalized;
    }
}
