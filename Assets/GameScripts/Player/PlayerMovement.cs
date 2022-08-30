using Mirror;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] public float speed = 15;
    [SerializeField] private ParticleSystem stepsParticleSystem;

    [SerializeField] private UIJoystick playerJoystick;
    private Rigidbody2D playerRigidBody;
    private Vector2 moveVelocity;

    private NetworkAnimator networkAnimator;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        networkAnimator = GetComponent<NetworkAnimator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void IncreaseSpeed(float amount)
    {
        speed += amount;
    }

    public void DecreaseSpeed(float amount)
    {
        if(speed > 1)
            speed -= amount;
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