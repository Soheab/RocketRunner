using UnityEngine;

public class MoveDownAndDestroy : MonoBehaviour
{
    [SerializeField] private float speed = 4f;
    [SerializeField] private float destroyY = -7f;
    [SerializeField] private float everySeconds = 10f;

    private float currentSpeedMultiplier;
    private float difficultyTimer;

    private void Start()
    {
        currentSpeedMultiplier = SettingsData.SpeedMultiplier;
    }

    private void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.Ended) return;

        difficultyTimer += Time.deltaTime;
        while (difficultyTimer >= everySeconds)
        {
            difficultyTimer -= everySeconds;
            currentSpeedMultiplier += SettingsData.ObstacleSpeedStepPer10s;
        }

        transform.position += Vector3.down * (speed * currentSpeedMultiplier * Time.deltaTime);

        if (transform.position.y < destroyY)
            Destroy(gameObject);
    }
}