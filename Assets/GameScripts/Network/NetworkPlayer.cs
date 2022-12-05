using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkPlayer : NetworkBehaviour
{
    [SyncVar] public Room CurrentRoom;
    [SyncVar] public string RoomID;

    public Action OnBeginGame;
    public Action OnStartGame;

    public static NetworkPlayer localPlayer;

    private bool disconected = false;

    private NetworkMatch networkMatch;

    private GameObject[] spawnPoints;

    private void Awake()
    {
        networkMatch = GetComponent<NetworkMatch>();
    }

    private void Start()
    {
        if (isClient)
        {
            Invoke(nameof(beginGame), 0.05f);
        }
    }

    private void beginGame()
    {
        OnBeginGame?.Invoke();
    }

    public override void OnStopServer()
    {
        if(!disconected)
            serverDisconnect();
    }

    public void DisconnectGame()
    {
        cmdDisconnectGame();
        UILobby.instance.disableSearchCanvas();
        SceneManager.UnloadSceneAsync(2);
    }

    [Command]
    private void cmdDisconnectGame()
    {
        serverDisconnect();
        NetworkServer.Destroy(gameObject);
    }

    private void serverDisconnect()
    {
        RoomList.instance.Rooms.Find(room => room.RoomId == RoomID).DisconnectPlayer(gameObject);
        disconected = true;
    }

    [TargetRpc]
    public void MoveToStartPoint(int playerIndex)
    {
        OnStartGame?.Invoke();
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        transform.position = spawnPoints[playerIndex].transform.position;
    }
}
