using UnityEngine;

public class TextureScroller : MonoBehaviour
{
    public float scrollSpeed = 0.5f; // how fast it moves
    private Renderer rend;
    private Vector2 offset;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        // Move in the opposite direction
        offset.y -= scrollSpeed * Time.deltaTime;

        rend.material.SetTextureOffset("_BaseMap", offset);
    }
}
