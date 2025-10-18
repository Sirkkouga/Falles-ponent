using UnityEngine;

public class WindBehaviour : MonoBehaviour
{
    [Header("Particle Prefabs")]
    public ParticleSystem windPrefab;
    public ParticleSystem windV2Prefab;

    [Header("Spawn Settings")]
    public int particlesPerSpawn = 4;     
    public float spawnInterval = 0.1f;    

    [Header("Wind Timing")]
    public float minInterval = 30f;       
    public float maxInterval = 50f;       
    public float windDuration = 10f;      

    [Header("Spawn Dispersion")]
    public float horizontalSpread = 0.5f; 
    public float verticalSpread = 3f;     

    [Header("Flame Link")]
    public FlameUI flameUI;               
    public float windBurnMultiplier = 5f; 
    public float burnEffectDelay = 2f;    // ⏳ Delay before flame starts burning faster

    private float waitTimer = 0f;
    private float eventTimer = 0f;
    private float spawnTimer = 0f;
    private float nextWaitTime = 0f;
    private bool windActive = false;
    private bool burnEffectActive = false;

    void Start()
    {
        ScheduleNextWind();
    }

    void Update()
    {
        if (!windActive)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= nextWaitTime)
            {
                StartWindEvent();
            }
        }
        else
        {
            eventTimer += Time.deltaTime;
            spawnTimer += Time.deltaTime;

            if (spawnTimer >= spawnInterval)
            {
                SpawnParticles();
                spawnTimer = 0f;
            }

            // 🔥 Activate burn boost after delay
            if (!burnEffectActive && eventTimer >= burnEffectDelay)
            {
                burnEffectActive = true;
                if (flameUI != null)
                    flameUI.externalBurnMultiplier = windBurnMultiplier;
            }

            // 🧎‍♂️ Allow player to cancel boost by crouching
            if (burnEffectActive && flameUI != null)
            {
                bool isCtrlHeld = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
                flameUI.externalBurnMultiplier = isCtrlHeld ? 1f : windBurnMultiplier;
            }

            // 🌬️ End of wind event
            if (eventTimer >= windDuration)
            {
                EndWindEvent();
            }
        }
    }

    void ScheduleNextWind()
    {
        waitTimer = 0f;
        nextWaitTime = Random.Range(minInterval, maxInterval);
    }

    void StartWindEvent()
    {
        windActive = true;
        eventTimer = 0f;
        spawnTimer = 0f;
        burnEffectActive = false;

        // Don’t touch flame yet — give player a reaction window
    }

    void EndWindEvent()
    {
        windActive = false;
        burnEffectActive = false;
        ScheduleNextWind();

        if (flameUI != null)
            flameUI.externalBurnMultiplier = 1f;
    }

    void SpawnParticles()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        float spawnZ = 0f;
        Vector3 rightEdge = cam.ViewportToWorldPoint(new Vector3(1, 0.5f, cam.nearClipPlane + spawnZ));

        for (int i = 0; i < particlesPerSpawn; i++)
        {
            float randomY = rightEdge.y + Random.Range(-verticalSpread / 2f, verticalSpread / 2f);
            float randomX = rightEdge.x + Random.Range(0f, horizontalSpread);
            Vector3 spawnPos = new Vector3(randomX, randomY, spawnZ);

            ParticleSystem prefab = (Random.value > 0.5f) ? windPrefab : windV2Prefab;
            ParticleSystem ps = Instantiate(prefab, spawnPos, Quaternion.Euler(0, 180, 0));
            ps.Play();
            Destroy(ps.gameObject, ps.main.startLifetime.constantMax + 5f);
        }
    }
}
