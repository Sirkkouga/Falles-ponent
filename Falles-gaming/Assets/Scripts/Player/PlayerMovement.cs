using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    bool isGrounded = true;
    public float jumpForce = 10.0f;
    public float moveSpeed = 5.0f;
    public float gravity = 9.0f;
    public float groundCheckDistance = 0.1f;

    public LayerMask groundLayer;
    private Rigidbody rb;


    void Start()
    {
        Debug.Log("PlayerMovement script has started.");
    }

    void Update()
    {

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            MovePlayer(Vector3.left * moveSpeed);
            Debug.Log("Player moved left.");
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            MovePlayer(Vector3.right * moveSpeed);
            Debug.Log("Player moved right.");
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            if (isGrounded)
            {
                PlayerJump();
                isGrounded = false;
                Debug.Log("Player is jumping.");
            }
            else {
                if (Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer))
                {
                    isGrounded = true;
                    Debug.Log("Player has landed and is grounded.");
                }
            }
        }
        // Player movement logic will go here
    }

    void MovePlayer(Vector3 direction)
    {
        transform.position += direction * Time.deltaTime;
    }

    void PlayerJump()
    {
        // Jump logic will go here
        transform.position += Vector3.up * jumpForce * Time.deltaTime;
        Debug.Log("Player jumped!");
    }

}


