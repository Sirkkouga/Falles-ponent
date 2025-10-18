using UnityEngine;

public class WindBehaviour : MonoBehaviour
{
    [Header("Particle Prefabs")]
    public ParticleSystem windPrefab;
    public ParticleSystem windV2Prefab;

    [Header("Spawn Settings")]
    public int particlesPerSpawn = 4;     // How many particles to spawn each spawn
    public float spawnInterval = 0.1f;    // Time between spawns during wind event

    [Header("Wind Timing")]
    public float minInterval = 30f;       // Min wait between wind events
    public float maxInterval = 50f;       // Max wait between wind events
    public float windDuration = 10f;      // Duration of a wind event in seconds

    [Header("Spawn Dispersion")]
    public float horizontalSpread = 0.5f; // X offset from right edge
    public float verticalSpread = 3f;     // Y variation around right edge center

    private float waitTimer = 0f;         // Timer for waiting between events
    private float eventTimer = 0f;        // Timer for current wind event
    private float spawnTimer = 0f;        // Timer for controlling spawn interval
    private float nextWaitTime = 0f;      // Random wait until next wind event
    private bool windActive = false;      // Is a wind event active?

    void Start()
    {
        ScheduleNextWind();
    }

    void Update()
    {
        if (!windActive)
        {
            // Waiting for next wind event
            waitTimer += Time.deltaTime;
            if (waitTimer >= nextWaitTime)
            {
                StartWindEvent();
            }
        }
        else
        {
            // Wind event active
            eventTimer += Time.deltaTime;
            spawnTimer += Time.deltaTime;

            // Spawn particles at fixed intervals during wind
            if (spawnTimer >= spawnInterval)
            {
                SpawnParticles();
                spawnTimer = 0f;
            }

            // End wind event after duration
            if (eventTimer >= windDuration)
            {
                EndWindEvent();
            }
        }
    }

    void ScheduleNextWind()
    {
        waitTimer = 0f;
        nextWaitTime = UnityEngine.Random.Range(minInterval, maxInterval);
    }

    void StartWindEvent()
    {
        windActive = true;
        eventTimer = 0f;
        spawnTimer = 0f;
    }

    void EndWindEvent()
    {
        windActive = false;
        ScheduleNextWind();
    }

    void SpawnParticles()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        float spawnZ = 0f; // Plane where particles live
        Vector3 rightEdge = cam.ViewportToWorldPoint(new Vector3(1, 0.5f, cam.nearClipPlane + spawnZ));

        for (int i = 0; i < particlesPerSpawn; i++)
        {
            // Random vertical offset
            float randomY = rightEdge.y + UnityEngine.Random.Range(-verticalSpread / 2f, verticalSpread / 2f);

            // Random horizontal offset along X
            float randomX = rightEdge.x + UnityEngine.Random.Range(0f, horizontalSpread);

            Vector3 spawnPos = new Vector3(randomX, randomY, spawnZ);

            // Randomly pick prefab
            ParticleSystem prefab = (UnityEngine.Random.value > 0.5f) ? windPrefab : windV2Prefab;

            ParticleSystem ps = Instantiate(prefab, spawnPos, Quaternion.Euler(0, 180, 0));
            ps.Play();

            Destroy(ps.gameObject, ps.main.startLifetime.constantMax + 5f);
        }
    }
}