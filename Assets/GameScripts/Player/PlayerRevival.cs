using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerRevival : NetworkBehaviour
{
    [SerializeField] private GameObject player;
    private NetworkPlayer networkPlayer;
    private Health health;
    private Collider2D playerCollider;

    private void Awake()
    {
        networkPlayer = player.GetComponent<NetworkPlayer>();
        health = player.GetComponent<Health>();
        playerCollider = player.GetComponent<Collider2D>();
    }

    private void Start()
    {
        if (hasAuthority)
            networkPlayer.OnBeginGame += revival;
    }

    private void OnDisable()
    {
        networkPlayer.OnBeginGame -= revival;
    }

    private void OnDestroy()
    {
        networkPlayer.OnBeginGame -= revival;
    }

    [Command]
    private void revival()
    {
        health.serverSetMaxHealthAmount();
        reviewOnServer();
    }
    
    [Server]
    private void reviewOnServer()
    {
        playerCollider.enabled = true;
    }
}
