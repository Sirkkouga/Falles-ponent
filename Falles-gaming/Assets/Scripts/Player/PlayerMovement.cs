using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    void Start()
    {
        Debug.Log("PlayerMovement script has started.");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space key was pressed.");
            PlayerJump();
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
        Debug.Log("Player jumped!");
    }

}


