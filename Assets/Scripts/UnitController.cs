using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitController : MonoBehaviour
{
    [Header("Public")]
    // Player speed
    public float speed = 2f;

    [Header("Private")]
    // Force player control
    [SerializeField] private bool forcePlayerControl = true;

    // Rigid body
    private Rigidbody rb;

    // Axes movement
    private float movementX;
    private float movementY;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get rigidbody from gameobject
        rb = GetComponent<Rigidbody>();
    }

    // Called once per fixed frame (for physics)
    private void FixedUpdate()
    {
        if (forcePlayerControl)
        {
            Vector3 movement = new Vector3(movementX, 0.0f, movementY);

            rb.AddForce(movement * speed);
        }
    }

    // Function is called when a move input is detected
    private void OnMove(InputValue movementValue)
    {
        // Get movement vector
        Vector2 movementVector = movementValue.Get<Vector2>();

        // Assign variables 
        movementX = movementVector.x;
        movementY = movementVector.y;
    }


    void OnTriggerEnter(Collider other)
    {
        // Check if the object the player collided with has the "PickUp" tag.
        if (other.gameObject.CompareTag("Collectible"))
        {
            // Deactivate the collided object (making it disappear).
            other.gameObject.SetActive(false);
        }
    }
}
