using Mirror;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private float speed = 15;

    [SerializeField] private UIJoystick playerJoystick;
    [SerializeField] private Canvas playerCanvas;

    [SerializeField] private GameObject playerCamera;
    private GameObject mainCamera;

    [SerializeField] private ParticleSystem stepsParticleSystem;

    private Rigidbody2D playerRigidBody;
    private Vector2 moveVelocity;
    private NetworkAnimator networkAnimator;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        playerRigidBody = GetComponent<Rigidbody2D>();
        networkAnimator = GetComponent<NetworkAnimator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    private void Update()
    {
        Vector2 moveInput = new Vector2(playerJoystick.HorizontalInput(), playerJoystick.VerticallInput());
        moveVelocity = moveInput * speed;
    }

    private void FixedUpdate()
    {
        spriteRenderer.sortingOrder = (int)(-transform.position.y * 100);

        if (isLocalPlayer)
        {
            setMovementAnimation();

            playerRigidBody.velocity = moveVelocity;
            changeParticleVector();
        }
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

    private void changeParticleVector()
    {
        if (playerRigidBody.velocity.magnitude != 0)
        {
            if (stepsParticleSystem.isStopped)
                stepsParticleSystem.Play();
        }
        else
        {
            stepsParticleSystem.Stop();
        }

        var velocityOverLifetime = stepsParticleSystem.velocityOverLifetime;
        velocityOverLifetime.x = playerJoystick.HorizontalInput() * -1f;
        velocityOverLifetime.y = playerJoystick.VerticallInput() * -1f;
    }
}