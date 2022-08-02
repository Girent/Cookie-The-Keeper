using Mirror;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private float speed = 10;

    [SerializeField] private UIJoystick playerJoystick;
    [SerializeField] private Canvas playerCanvas;

    [SerializeField] private GameObject playerCamera;
    private GameObject mainCamera;

    private Rigidbody2D playerRigidBody;
    private Vector2 moveVelocity;
    private NetworkAnimator networkAnimator;

    private void Awake()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        playerRigidBody = GetComponent<Rigidbody2D>();
        networkAnimator = GetComponent<NetworkAnimator>();
    }
    
    private void Update()
    {
        Vector2 moveInput = new Vector2(playerJoystick.HorizontalInput(), playerJoystick.VerticallInput());
        moveVelocity = moveInput * speed;
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer)
            return;

        setMovementAnimation();

        playerRigidBody.velocity = moveVelocity;
    }

    private void setMovementAnimation()
    {
        networkAnimator.animator.SetBool("IsMove", playerRigidBody.velocity.magnitude != 0);
        networkAnimator.animator.SetFloat("Horizontal", playerRigidBody.velocity.x);
        networkAnimator.animator.SetFloat("Vertical", playerRigidBody.velocity.y);
    }

    public void EnablePlayerInterface()
    {
        mainCamera.SetActive(false);

        if (isLocalPlayer)
            playerCamera.SetActive(true);

        if (hasAuthority)
            playerCanvas.enabled = true;
    }

    public void DisablePlayerInterface()
    {
        mainCamera.SetActive(true);

        if (isLocalPlayer)
            playerCamera.SetActive(false);

        if (hasAuthority)
            playerCanvas.enabled = false;
    }
}