using UnityEngine;

public class TextureScroller : MonoBehaviour
{
    public float scrollSpeed = 0.5f;      // Normal speed
    public float boostMultiplier = 2.0f;  // Speed multiplier when Shift is held
    private Renderer rend;
    private Vector2 offset;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        // Left/Right input
        float horizontalInput = Input.GetAxisRaw("Horizontal"); // -1 left, +1 right

        // Check if Left Shift or Right Shift is held
        bool isShiftHeld = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        // Calculate speed (boosted if shift is held)
        float currentSpeed = scrollSpeed * (isShiftHeld ? boostMultiplier : 1f);

        // Shift texture along V coordinate (offset.y)
        offset.y -= horizontalInput * currentSpeed * Time.deltaTime;

        // Apply to URP material
        rend.material.SetTextureOffset("_BaseMap", offset);
    }
}


