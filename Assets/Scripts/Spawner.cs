using System;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private enum ObstaclePattern
    {
        FlatLine,
        DiagonalRise,
        DiagonalDrop,
        VShape,
        ZigZag,
        Cluster
    }

    [Header("Prefab Pools")]
    [SerializeField] private GameObject[] obstaclePrefabs;
    [SerializeField] private GameObject[] pickupPrefabs;

    [Header("Fallback Prefabs")]
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private GameObject pickupPrefab;

    [Header("Spawn Area")]
    [SerializeField] private float xRange = 8f;
    [SerializeField] private float ySpawn = 6f;

    [Header("Obstacle Placement")]
    [SerializeField] private float minObstacleXSpacing = 1.5f;
    [SerializeField] private float patternXStep = 1.1f;
    [SerializeField] private float patternYStep = 0.65f;

    [Header("Difficulty Timing")]
    [SerializeField] private float everySeconds = 10f;

    private float spawnTimer;
    private float difficultyTimer;
    private float currentRate;

    private int consecutiveObstacleSpawns;
    private int consecutivePickupSpawns;

    private void Start()
    {
        currentRate = Mathf.Max(SettingsData.SpawnMinRate, SettingsData.SpawnStartRate);
    }

    private void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.Ended) return;

        spawnTimer += Time.deltaTime;
        difficultyTimer += Time.deltaTime;

        if (spawnTimer >= currentRate)
        {
            spawnTimer = 0f;
            SpawnOne();
        }

        ApplyDifficultySteps();
    }

    private void ApplyDifficultySteps()
    {
        if (everySeconds <= 0f) return;

        while (difficultyTimer >= everySeconds)
        {
            difficultyTimer -= everySeconds;

            float step = Mathf.Max(0f, SettingsData.SpawnRateStepPer10s);

            if (SettingsData.DifficultyIndex == 2 &&
                GameManager.Instance != null &&
                SettingsData.RoundTimeSeconds - GameManager.Instance.TimeLeft >= SettingsData.HardExtraPressureAfterSeconds)
            {
                step *= Mathf.Max(1f, SettingsData.HardSpawnStepMultiplierAfterPressure);
            }

            float minRate = Mathf.Max(0.05f, SettingsData.SpawnMinRate);
            currentRate = Mathf.Max(minRate, currentRate - step);
        }
    }

    private void SpawnOne()
    {
        if (!HasAnyPrefab(obstaclePrefabs, obstaclePrefab) && !HasAnyPrefab(pickupPrefabs, pickupPrefab))
        {
            Debug.LogWarning("Spawner: no prefabs assigned");
            return;
        }

        int maxConsecutiveObstacles = Mathf.Max(1, SettingsData.MaxConsecutiveObstacles);
        float pickupChance = Mathf.Clamp01(SettingsData.PickupChance);

        bool forcePickup = consecutiveObstacleSpawns >= maxConsecutiveObstacles;
        bool forceObstacle = consecutivePickupSpawns >= 2;

        bool spawnPickup = !forceObstacle && (forcePickup || UnityEngine.Random.value < pickupChance);

        if (spawnPickup)
        {
            SpawnPickup();
            return;
        }

        SpawnObstacleBurst();
    }

    private void SpawnPickup()
    {
        GameObject prefab = PickRandomPrefab(pickupPrefabs, pickupPrefab);
        if (prefab == null)
        {
            Debug.LogWarning("Spawner: no pickup prefab assigned");
            return;
        }

        consecutiveObstacleSpawns = 0;
        consecutivePickupSpawns++;

        float x = UnityEngine.Random.Range(-xRange, xRange);
        Instantiate(prefab, new Vector3(x, ySpawn, 0f), Quaternion.identity);
    }

    private void SpawnObstacleBurst()
    {
        bool allowLaser = SettingsData.DifficultyIndex == 2;
        GameObject fallback = PickRandomObstaclePrefab(allowLaser);
        if (fallback == null)
        {
            Debug.LogWarning("Spawner: no obstacle prefab assigned");
            return;
        }

        consecutivePickupSpawns = 0;
        consecutiveObstacleSpawns++;

        int minBurst = Mathf.Max(1, SettingsData.ObstacleBurstMin);
        int maxBurst = Mathf.Max(minBurst, SettingsData.ObstacleBurstMax);
        int burstCount = UnityEngine.Random.Range(minBurst, maxBurst + 1);
        burstCount = Mathf.Clamp(burstCount, 1, 6);

        ObstaclePattern pattern = (ObstaclePattern)UnityEngine.Random.Range(0, Enum.GetValues(typeof(ObstaclePattern)).Length);
        List<Vector2> offsets = BuildPatternOffsets(pattern, burstCount);

        var usedX = new List<float>(burstCount);
        float centerX = GetSpacedSpawnX(usedX);

        for (int i = 0; i < burstCount; i++)
        {
            GameObject prefab = PickRandomObstaclePrefab(allowLaser);
            if (prefab == null) prefab = fallback;

            Vector2 off = offsets[i];
            float x = ResolvePatternSpawnX(centerX + off.x, usedX);
            usedX.Add(x);

            Vector3 pos = new Vector3(x, ySpawn + off.y, 0f);
            Instantiate(prefab, pos, Quaternion.identity);
        }
    }

    private static bool HasAnyPrefab(GameObject[] list, GameObject fallback)
    {
        if (fallback != null) return true;
        if (list == null) return false;
        for (int i = 0; i < list.Length; i++)
            if (list[i] != null) return true;
        return false;
    }

    private static GameObject PickRandomPrefab(GameObject[] list, GameObject fallback)
    {
        if (list != null && list.Length > 0)
        {
            int count = 0;
            for (int i = 0; i < list.Length; i++)
                if (list[i] != null) count++;

            if (count > 0)
            {
                int pick = UnityEngine.Random.Range(0, count);
                int seen = 0;

                for (int i = 0; i < list.Length; i++)
                {
                    if (list[i] == null) continue;
                    if (seen == pick) return list[i];
                    seen++;
                }
            }
        }

        return fallback;
    }

    private GameObject PickRandomObstaclePrefab(bool allowLaser)
    {
        var candidates = new List<GameObject>();

        if (obstaclePrefabs != null && obstaclePrefabs.Length > 0)
        {
            for (int i = 0; i < obstaclePrefabs.Length; i++)
            {
                GameObject prefab = obstaclePrefabs[i];
                if (prefab == null) continue;
                if (!allowLaser && IsLaserObstaclePrefab(prefab)) continue;
                candidates.Add(prefab);
            }
        }

        if (candidates.Count > 0)
            return candidates[UnityEngine.Random.Range(0, candidates.Count)];

        if (obstaclePrefab != null && (allowLaser || !IsLaserObstaclePrefab(obstaclePrefab)))
            return obstaclePrefab;

        return null;
    }

    private static bool IsLaserObstaclePrefab(GameObject prefab)
    {
        return prefab != null && prefab.name.ToLowerInvariant().Contains("laser");
    }

    private float GetSpacedSpawnX(List<float> usedX)
    {
        for (int attempt = 0; attempt < 8; attempt++)
        {
            float candidate = UnityEngine.Random.Range(-xRange, xRange);
            if (!Overlaps(candidate, usedX)) return candidate;
        }
        return UnityEngine.Random.Range(-xRange, xRange);
    }

    private float ResolvePatternSpawnX(float targetX, List<float> usedX)
    {
        float candidate = Mathf.Clamp(targetX, -xRange, xRange);

        for (int attempt = 0; attempt < 6; attempt++)
        {
            if (!Overlaps(candidate, usedX)) return candidate;

            float dir = (attempt % 2 == 0) ? 1f : -1f;
            float shift = (attempt + 1) * minObstacleXSpacing * 0.55f;
            candidate = Mathf.Clamp(targetX + dir * shift, -xRange, xRange);
        }

        return GetSpacedSpawnX(usedX);
    }

    private bool Overlaps(float x, List<float> usedX)
    {
        for (int i = 0; i < usedX.Count; i++)
        {
            if (Mathf.Abs(x - usedX[i]) < minObstacleXSpacing)
                return true;
        }
        return false;
    }

    private List<Vector2> BuildPatternOffsets(ObstaclePattern pattern, int count)
    {
        var offsets = new List<Vector2>(count);

        float mid = (count - 1) * 0.5f;
        float xStep = Mathf.Max(0.35f, patternXStep);
        float yStep = Mathf.Max(0.2f, patternYStep);

        for (int i = 0; i < count; i++)
        {
            float x = (i - mid) * xStep;
            float y = 0f;

            switch (pattern)
            {
                case ObstaclePattern.FlatLine:
                    y = 0f;
                    break;

                case ObstaclePattern.DiagonalRise:
                    y = i * yStep;
                    break;

                case ObstaclePattern.DiagonalDrop:
                    y = (count - 1 - i) * yStep;
                    break;

                case ObstaclePattern.VShape:
                    y = Mathf.Abs(i - mid) * yStep * 0.8f;
                    break;

                case ObstaclePattern.ZigZag:
                    y = (i % 2 == 0) ? 0f : yStep;
                    break;

                case ObstaclePattern.Cluster:
                    x = UnityEngine.Random.Range(-xStep, xStep);
                    y = UnityEngine.Random.Range(0f, yStep * 1.5f);
                    break;
            }

            offsets.Add(new Vector2(x, y));
        }

        return offsets;
    }
}
