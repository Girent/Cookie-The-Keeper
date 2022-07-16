using Mirror;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private float speed = 5;
    [SerializeField] private Joystick playerJoystick;
    [SerializeField] private GameObject playerCamera;

    [SerializeField] private Canvas playerCanvas;

    private GameObject mainCamera;

    private Rigidbody2D rb;
    private Vector2 moveVelocity;

    private void Awake()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

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

    public void EnablePlayerInterface()
    {
        mainCamera.SetActive(false);
        if (isLocalPlayer)
        {
            playerCamera.SetActive(true);
        }
        if (hasAuthority)
            playerCanvas.enabled = true;
    }
}