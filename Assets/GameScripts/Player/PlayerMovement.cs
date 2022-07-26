using Mirror;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private float speed = 5;
    [SerializeField] private UIJoystick playerJoystick;
    [SerializeField] private GameObject playerCamera;

    [SerializeField] private Canvas playerCanvas;

    private GameObject mainCamera;

    private Rigidbody2D playerRigidBody;
    private Vector2 moveVelocity;

    private void Awake()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        playerRigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector2 moveInput = new Vector2(playerJoystick.HorizontalInput(), playerJoystick.VerticallInput());
        moveVelocity = moveInput * speed;
    }

    private void FixedUpdate()
    {
        playerRigidBody.MovePosition(playerRigidBody.position + moveVelocity * Time.deltaTime);
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

    public void DisablePlayerInterface()
    {
        mainCamera.SetActive(true);
        if (isLocalPlayer)
        {
            playerCamera.SetActive(false);
        }
        if (hasAuthority)
            playerCanvas.enabled = false;
    }
}