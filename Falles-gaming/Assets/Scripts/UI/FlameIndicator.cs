using UnityEngine;
using UnityEngine.UI;

public class FlameUI : MonoBehaviour
{
    public Image flameImage;

    public float baseBurnTime = 10f;    // Full burn when idle
    public float walkMultiplier = 1.5f; // Burns faster when walking
    public float runMultiplier = 3f;    // Burns fastest when running

    private float burnProgress = 0f;    // 0 = full, 1 = empty

    void Start()
    {
        flameImage.fillAmount = 1f;
    }

    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        bool isMoving = horizontalInput != 0;
        bool isRunning = isMoving && Input.GetKey(KeyCode.LeftShift);

        // Calculate burn rate per second
        float burnRate = 1f / baseBurnTime; // base rate

        if (isRunning)
            burnRate *= runMultiplier;
        else if (isMoving)
            burnRate *= walkMultiplier;

        // Increase progress smoothly
        burnProgress += burnRate * Time.deltaTime;
        burnProgress = Mathf.Clamp01(burnProgress);

        // Apply to flame fill
        flameImage.fillAmount = 1f - burnProgress;
    }

    // Optional: reset the flame
    public void ResetFlame()
    {
        burnProgress = 0f;
        flameImage.fillAmount = 1f;
    }
}
