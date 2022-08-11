using Mirror;
using UnityEngine;

public class PlayerDead : NetworkBehaviour
{
    [SerializeField] private Health health;
    private PlayerMovement playerMovement;

    private void OnEnable()
    {
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

    private void die()
    {
        Debug.Log("jj");
        playerMovement.enabled = false;
    }
}
