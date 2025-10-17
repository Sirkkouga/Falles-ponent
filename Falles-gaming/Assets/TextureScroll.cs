using UnityEngine;

public class TextureScroller : MonoBehaviour
{
    public float scrollSpeed = 0.5f; // How fast the texture shifts
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

        // Shift along V coordinate (offset.y) to match world X movement
        offset.y -= horizontalInput * scrollSpeed * Time.deltaTime;

        // Apply to URP material
        rend.material.SetTextureOffset("_BaseMap", offset);
    }
}
