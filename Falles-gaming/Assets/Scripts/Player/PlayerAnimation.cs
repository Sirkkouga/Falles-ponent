using UnityEngine;

public class PlayerSpriteAnimator : MonoBehaviour
{
    [Header("Idle / Walk Sprites")]
    public Texture parado;      // Idle texture
    public Texture camina1;     // Walk frame 1
    
    public Texture camina2;     // Walk frame 2

    [Header("Run Sprites")]
    public Texture correr1;     // Run frame 1
    public Texture correr2;     // Run frame 2
    public Texture correr3;     // Run frame 3
    public Texture correr4;     // Run frame 4

    public float walkAnimationSpeed = 0.2f; // Seconds per frame
    public float runAnimationSpeed = 0.1f;  // Faster animation for running

    private Renderer rend;
    private float timer;
    private int walkFrameIndex = 0;
    private int runFrameIndex = 0;


    public AudioSource audioSource;
    public AudioClip tolon_tolon;

    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material.SetTexture("_MainTex", parado);
        rend.material.mainTextureScale = new Vector2(1f, 1f); // default scale
        rend.material.mainTextureOffset = Vector2.zero;       // default offset
    }

    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        bool isShiftHeld = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        // --- No movement: idle ---
        if (horizontalInput == 0)
        {
            rend.material.SetTexture("_MainTex", parado);
            timer = 0f;
            walkFrameIndex = 0;
            runFrameIndex = 0;

            // Stop audio if playing
            if (audioSource.isPlaying)
                audioSource.Stop();
        }
        else
        {
            // --- Walking or Running Animation ---
            timer += Time.deltaTime;

            if (isShiftHeld)
            {
                // Running
                if (timer >= runAnimationSpeed)
                {
                    timer = 0f;
                    runFrameIndex = (runFrameIndex + 1) % 4; // 4 running frames

                    switch (runFrameIndex)
                    {
                        case 0: rend.material.SetTexture("_MainTex", correr1); break;
                        case 1: rend.material.SetTexture("_MainTex", correr4); break;
                        case 2: rend.material.SetTexture("_MainTex", correr2); break;
                        case 3: rend.material.SetTexture("_MainTex", correr3); break;
                    }
                }
            }
            else
            {
                // Walking
                if (timer >= walkAnimationSpeed)
                {
                    timer = 0f;
                    walkFrameIndex = (walkFrameIndex + 1) % 4; // 4 walking frames
                    switch (walkFrameIndex)
                    {
                        case 0: rend.material.SetTexture("_MainTex", camina1); break;
                        case 1: rend.material.SetTexture("_MainTex", parado); break;
                        case 2: rend.material.SetTexture("_MainTex", camina2); break;
                        case 3: rend.material.SetTexture("_MainTex", parado); break;
                    }
                }
                // Play audio if not already playing
                if (!audioSource.isPlaying)
                {
                    audioSource.clip = tolon_tolon;
                    audioSource.Play();
                }
            }
        }

        // --- Flip Texture Horizontally ---
        Vector2 scale = rend.material.mainTextureScale;
        Vector2 offset = rend.material.mainTextureOffset;

        if (horizontalInput >= 0)
        {
            scale.x = 1f;
            offset.x = 0f;
        }
        else
        {
            scale.x = -1f;
            offset.x = 1f; // shift texture so it stays visible
        }

        rend.material.mainTextureScale = scale;
        rend.material.mainTextureOffset = offset;
    }
}
