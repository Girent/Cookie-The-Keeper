using Mirror;
using UnityEngine;

public class PlayerDead : NetworkBehaviour
{
    [SerializeField] private GameObject player;
    private Collider2D playerCollider;
    private Health health;
    private NetworkAnimator playerAnimator;

    private void Awake()
    {
        playerCollider = player.GetComponent<Collider2D>();
        health = player.GetComponent<Health>();
        playerAnimator = player.GetComponent<NetworkAnimator>();
    }

    private void Start()
    {
        if (hasAuthority)
            health.OnPlayerDead += die;
    }

    private void OnDisable()
    {
        health.OnPlayerDead -= die;
    }

    private void OnDestroy()
    {
        health.OnPlayerDead -= die;
    }

    [Client]
    private void die()
    {
        playerAnimator.SetTrigger("Dead");
        cmdDie();
    }


    [Command]
    private void cmdDie()
    {
        inServerDie();
    }

    [Server]
    private void inServerDie()
    {
    }
}
