using Mirror;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed = 5;
    [SerializeField] private Joystick playerJoystick;

    private Rigidbody2D rb;
    private Vector2 moveVelocity;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        Vector2 moveInput = new Vector2(playerJoystick.Horizontal, playerJoystick.Vertical);
        moveVelocity = moveInput * speed;
        
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveVelocity * Time.deltaTime);
    }
}