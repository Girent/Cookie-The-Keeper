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

    public bool InGame = false;

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

    #region DisconectMatch
    public void DisconnectGame()
    {
        cmdDisconnectGame();
        UILobby.instance.disableSearchCanvas();
        SceneManager.UnloadSceneAsync(2);
        NetworkServer.Destroy(gameObject);
    }

    [Command]
    private void cmdDisconnectGame()
    {
        serverDisconnect();
    }

    private void serverDisconnect()
    {
        RoomList.instance.Rooms.Find(room => room.RoomId == RoomID).DisconnectPlayer(this);
        InGame = false;
        disconected = true;
    }
    #endregion

    #region Start Match

    [TargetRpc]
    public void MoveToStartPoint(int playerIndex)
    {
        OnStartGame?.Invoke();
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        transform.position = spawnPoints[playerIndex].transform.position;
    }

    #endregion
}
