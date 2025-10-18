using UnityEngine;
using UnityEngine.UI;

public class FlameUI : MonoBehaviour
{
    [Header("UI")]
    public Image flameImage; // Reference to the UI flame icon

    [Header("Light Control")]
    public Light flameLight;                // Reference to the light in the scene
    public float maxLightIntensity = 5f;    // Full intensity when flame full
    public float minLightIntensity = 0.5f;  // Minimum intensity when flame is out

    [Header("Flame Prefab Control")]
    public Transform flamePrefab;           // The in-world flame prefab (e.g. fire model or particle parent)
    public float maxPrefabScale = 1f;       // Full size when flame is full
    public float minPrefabScale = 0f;       // Minimum size (completely gone)

    [Header("Burn Settings")]
    public float baseBurnTime = 10f;        // Full burn when idle
    public float walkMultiplier = 1.5f;     // Burns faster when walking
    public float runMultiplier = 3f;        // Burns fastest when running
    public float externalBurnMultiplier = 1f; // Multiplied externally by wind, etc.

    private float burnProgress = 0f;        // 0 = full, 1 = empty

    void Start()
    {
        if (flameImage != null)
            flameImage.fillAmount = 1f;

        if (flameLight != null)
            flameLight.intensity = maxLightIntensity;

        if (flamePrefab != null)
            flamePrefab.localScale = Vector3.one * maxPrefabScale;
    }

    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        bool isMoving = horizontalInput != 0;
        bool isRunning = isMoving && Input.GetKey(KeyCode.LeftShift);

        // --- Calculate burn rate per second ---
        float burnRate = (1f / baseBurnTime) * externalBurnMultiplier; // base rate

        if (isRunning)
            burnRate *= runMultiplier;
        else if (isMoving)
            burnRate *= walkMultiplier;

        // --- Update burn progress ---
        burnProgress += burnRate * Time.deltaTime;
        burnProgress = Mathf.Clamp01(burnProgress);

        // --- Update UI flame ---
        if (flameImage != null)
            flameImage.fillAmount = 1f - burnProgress;

        // --- Update Light intensity ---
        if (flameLight != null)
        {
            float t = 1f - burnProgress;
            flameLight.intensity = Mathf.Lerp(minLightIntensity, maxLightIntensity, t);
        }

        // --- Update Flame Prefab scale ---
        if (flamePrefab != null)
        {
            float t = 1f - burnProgress; // t = 1 when full, 0 when empty
            float newScale = Mathf.Lerp(minPrefabScale, maxPrefabScale, t);
            flamePrefab.localScale = Vector3.one * newScale;

            // Hide prefab completely when empty
            flamePrefab.gameObject.SetActive(newScale > 0.01f);
        }
    }

    // --- Optional: reset the flame ---
    public void ResetFlame()
    {
        burnProgress = 0f;

        if (flameImage != null)
            flameImage.fillAmount = 1f;

        if (flameLight != null)
            flameLight.intensity = maxLightIntensity;

        if (flamePrefab != null)
        {
            flamePrefab.localScale = Vector3.one * maxPrefabScale;
            flamePrefab.gameObject.SetActive(true);
        }
    }
}
