using UnityEngine;

public class SpriteDynamic : MonoBehaviour
{
    private Vector3 originalPosition;
    private Transform playerParent;

    void Start()
    {
        // Save original position
        originalPosition = transform.localPosition;

        // Find parent Quad named "Jugador"
        playerParent = transform.parent;
        if (playerParent == null || playerParent.name != "Jugador")
        {
            Debug.LogWarning("Parent object Jugador not found. Flipping might not work correctly.");
        }
    }

    void Update()
    {
        // --- Move down while Ctrl is held ---
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            transform.localPosition = originalPosition + Vector3.down * 0.2f;
        }
        else
        {
            transform.localPosition = originalPosition;
        }

        // --- Flip horizontally when moving left ---
        float horizontalInput = Input.GetAxisRaw("Horizontal"); // -1 left, +1 right
        if (playerParent != null)
        {
            Vector3 localScale = transform.localScale;
            localScale.x = horizontalInput < 0 ? -Mathf.Abs(localScale.x) : Mathf.Abs(localScale.x);
            transform.localScale = localScale;
        }
    }
}
