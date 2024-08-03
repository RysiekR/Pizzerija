using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] CharacterController characterController;
    [SerializeField] GameObject PlayerVisual;
    private float normalSpeed = 5f;
    private float sprintSpeed = 20f;
    private float currentSpeed;


    private void Update()
    {
        Movement();
        CameraRotation();
    }
    /// <summary>
    /// Movement on XZ axis with Y dropforce (with Input.GetAxis)
    /// Shift for sprint
    /// </summary>
    private void Movement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        bool shiftDown = Input.GetKey(KeyCode.LeftShift);
        currentSpeed = shiftDown ? sprintSpeed : normalSpeed;

        Vector3 movement = transform.right * moveX + transform.forward * moveY;
        Vector3 move = movement * currentSpeed * Time.deltaTime;
        move.y = -2f;
        characterController.Move(move);

        if (movement != Vector3.zero)
        {
            PlayerVisualRotation(movement);
        }

    }
    /// <summary>
    /// Rotate whole Player gameObject (with Q/E Keys)
    /// </summary>
    private void CameraRotation()
    {
        bool leftRotation = false;
        bool rightRotation = false;
        float rotationSpeed = Input.GetKey(KeyCode.LeftShift) ? 150f : 50f;

        if (Input.GetKey(KeyCode.Q)) leftRotation = true;
        if (Input.GetKey(KeyCode.E)) rightRotation = true;

        if (leftRotation)
        {
            transform.Rotate(0, -rotationSpeed * Time.deltaTime, 0);
        }
        if (rightRotation)
        {
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        }

    }
    /// <summary>
    /// Rotate Player Visual using Vector3 of moveDirection as input
    /// </summary>
    private void PlayerVisualRotation(Vector3 movementDirection)
    {
        //float stepSpeed = currentSpeed * 5 * Time.deltaTime;
        float multiplayer = Input.GetKey(KeyCode.LeftShift) ? 30 : 40; // first in sprint(smaller bcs of regular movement speed)
        float stepSpeed = currentSpeed * multiplayer * Time.deltaTime;
        Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
        PlayerVisual.transform.rotation = Quaternion.RotateTowards(PlayerVisual.transform.rotation, targetRotation, stepSpeed);
    }

}
