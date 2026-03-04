public static class SettingsData
{
    public static int DifficultyIndex = 1;
    public static float RoundTimeSeconds = 60f;
    public static float PlayerSpeed = 6f;
    public static int WinScore = 20;

    public static float SpawnStartRate = 1.2f;
    public static float SpawnMinRate = 0.45f;
    public static float SpeedMultiplier = 1f;
    public static float SpawnRateStepPer10s = 0.08f;
    public static float PlayerSpeedStepPer10s = 0.2f;
    public static float ObstacleSpeedStepPer10s = 0.1f;
    public static float PickupChance = 0.35f;
    public static int MaxConsecutiveObstacles = 2;
    public static int ObstacleBurstMin = 1;
    public static int ObstacleBurstMax = 1;
    public static float HardExtraPressureAfterSeconds = 30f;
    public static float HardSpawnStepMultiplierAfterPressure = 1f;


    public static string GetScoreLabelText(int? scoreOverride = null)
    {
        int currentScore = scoreOverride ?? (GameManager.Instance != null ? GameManager.Instance.Score : 0);
        return "Score: " + currentScore + " / " + (WinScore == 0 ? "Tijdgebaseerd" : WinScore.ToString());
    }

    public static void ApplyDifficulty()
    {
        switch (DifficultyIndex)
        {
            case 0: // makkelijk
                SpawnStartRate = 1.6f;
                SpawnMinRate = 0.9f;
                SpeedMultiplier = 0.85f;
                SpawnRateStepPer10s = 0.05f;
                PlayerSpeedStepPer10s = 0.15f;
                ObstacleSpeedStepPer10s = 0.08f;
                PickupChance = 0.6f;
                MaxConsecutiveObstacles = 3;
                ObstacleBurstMin = 1;
                ObstacleBurstMax = 3;
                HardExtraPressureAfterSeconds = 30f;
                HardSpawnStepMultiplierAfterPressure = 1f;
                break;
            case 2: // moeilijk
                SpawnStartRate = 1.0f;
                SpawnMinRate = 0.35f;
                SpeedMultiplier = 1.25f;
                SpawnRateStepPer10s = 0.1f;
                PlayerSpeedStepPer10s = 0.08f;
                ObstacleSpeedStepPer10s = 0.14f;
                PickupChance = 0.35f;
                MaxConsecutiveObstacles = 3;
                ObstacleBurstMin = 2;
                ObstacleBurstMax = 3;
                HardExtraPressureAfterSeconds = 35f;
                HardSpawnStepMultiplierAfterPressure = 1.2f;
                break;
            default:
                SpawnStartRate = 1.05f;
                SpawnMinRate = 0.4f;
                SpeedMultiplier = 1.15f;
                SpawnRateStepPer10s = 0.09f;
                PlayerSpeedStepPer10s = 0.12f;
                ObstacleSpeedStepPer10s = 0.12f;
                PickupChance = 0.45f;
                MaxConsecutiveObstacles = 3;
                ObstacleBurstMin = 2;
                ObstacleBurstMax = 3;
                HardExtraPressureAfterSeconds = 30f;
                HardSpawnStepMultiplierAfterPressure = 1f;
                break;
        }
    }
}
