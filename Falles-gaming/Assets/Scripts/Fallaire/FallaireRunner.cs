using UnityEngine;

public class AutoRunnerAnimator : MonoBehaviour
{
    [Header("Run Sprites")]
    public Texture correr1;
    public Texture correr2;
    public Texture correr3;
    public Texture correr4;

    [Header("Animation Speed")]
    public float runAnimationSpeed = 0.1f;

    [Header("Movement Settings")]
    public Transform mainPlayer;        // Reference to the main player
    public float speedStatic = 5f;      // NPC moves away fast when player is static
    public float speedWalkRight = 2f;        // NPC moves away slower when player is walking
    public float speedRunRight = -1f;        // NPC moves slightly toward player when player is running
    public float speedWalkLeft = 5f;        // NPC moves away slower when player is walking
    public float speedRunLeft = 8f;        // NPC moves slightly toward player when player is running
    public float EscapeRun = 8f;

    [Header("Floor Slope")]
    public float floorZAngle = 10f;     // Slope in degrees

    [Header("Flame UI")]
    public FlameUI flameUI; // assign in Inspector


    private Renderer rend;
    private float timer = 0f;
    private int runFrameIndex = 0;

    private Vector3 slopeDirection;

    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material.SetTexture("_MainTex", correr1);
        rend.material.mainTextureScale = new Vector2(1f, 1f);
        rend.material.mainTextureOffset = Vector2.zero;

        // Precompute the slope movement direction
        float radians = floorZAngle * Mathf.Deg2Rad;
        slopeDirection = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0f).normalized;
    }

    void Update()
    {
        // --- Animate running ---
        timer += Time.deltaTime;
        if (timer >= runAnimationSpeed)
        {
            timer = 0f;
            runFrameIndex = (runFrameIndex + 1) % 4;

            switch (runFrameIndex)
            {
                case 0: rend.material.SetTexture("_MainTex", correr1); break;
                case 1: rend.material.SetTexture("_MainTex", correr4); break;
                case 2: rend.material.SetTexture("_MainTex", correr2); break;
                case 3: rend.material.SetTexture("_MainTex", correr3); break;
            }
        }

        // --- Move NPC based on player's input along slope ---
        if (mainPlayer != null)
        {
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            bool isReversed = horizontalInput < 0f;
            bool isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            float moveDelta = 0f;

            if (horizontalInput == 0)
                moveDelta = speedStatic * Time.deltaTime;
            else if (!isRunning)
                moveDelta = isReversed ? speedWalkLeft * Time.deltaTime : speedWalkRight * Time.deltaTime;
            else
                moveDelta = isReversed ? speedRunLeft * Time.deltaTime : speedRunRight * Time.deltaTime;

            // Move along the slope
            transform.position += slopeDirection * moveDelta;
        }

        // --- Always face right ---
        Vector2 scale = rend.material.mainTextureScale;
        Vector2 offset = rend.material.mainTextureOffset;
        scale.x = 1f;
        offset.x = 0f;
        rend.material.mainTextureScale = scale;
        rend.material.mainTextureOffset = offset;
    }

    // --- Trigger detection for flame reset ---
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // your player tag/name
        {
            if (flameUI != null)
            {
                flameUI.ResetFlame();
                Debug.Log("Player touched NPC: Flame reset!");
            }
        }
        if (other.CompareTag("Player")) // your player tag/name
        {
            if (flameUI != null)
            {
                flameUI.ResetFlame();
                Debug.Log("Player touched NPC: Flame reset!");
            }

            // Start escape movement for 10 seconds
            StartCoroutine(EscapeRoutine());
        }

        System.Collections.IEnumerator EscapeRoutine()
        {
            float elapsed = 0f;
            const float duration = 5f;
            while (elapsed < duration)
            {
                float moveDelta = EscapeRun * Time.deltaTime;
                transform.position += slopeDirection * moveDelta;
                elapsed += Time.deltaTime;
                yield return null;
            }
        }
    }
}

