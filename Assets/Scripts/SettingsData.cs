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

    public static void ApplyDifficulty()
    {
        switch (DifficultyIndex)
        {
            case 0: // makkelijk
                SpawnStartRate = 1.6f;
                SpawnMinRate = 0.9f;
                SpeedMultiplier = 0.85f;
                SpawnRateStepPer10s = 0.04f;
                PlayerSpeedStepPer10s = 0.15f;
                ObstacleSpeedStepPer10s = 0.08f;
                PickupChance = 0.5f;
                MaxConsecutiveObstacles = 1;
                ObstacleBurstMin = 1;
                ObstacleBurstMax = 2;
                HardExtraPressureAfterSeconds = 30f;
                HardSpawnStepMultiplierAfterPressure = 1f;
                break;
            case 2: // moeilijk
                SpawnStartRate = 0.9f;
                SpawnMinRate = 0.25f;
                SpeedMultiplier = 1.35f;
                SpawnRateStepPer10s = 0.12f;
                PlayerSpeedStepPer10s = 0.08f;
                ObstacleSpeedStepPer10s = 0.16f;
                PickupChance = 0.2f;
                MaxConsecutiveObstacles = 4;
                ObstacleBurstMin = 3;
                ObstacleBurstMax = 4;
                HardExtraPressureAfterSeconds = 30f;
                HardSpawnStepMultiplierAfterPressure = 1.35f;
                break;
            default:
                SpawnStartRate = 1.05f;
                SpawnMinRate = 0.4f;
                SpeedMultiplier = 1.15f;
                SpawnRateStepPer10s = 0.09f;
                PlayerSpeedStepPer10s = 0.12f;
                ObstacleSpeedStepPer10s = 0.12f;
                PickupChance = 0.3f;
                MaxConsecutiveObstacles = 3;
                ObstacleBurstMin = 2;
                ObstacleBurstMax = 3;
                HardExtraPressureAfterSeconds = 30f;
                HardSpawnStepMultiplierAfterPressure = 1f;
                break;
        }
    }
}