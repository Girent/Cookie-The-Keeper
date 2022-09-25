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
    public Action OnDisconnectGame;

    public static NetworkPlayer localPlayer;

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
            Invoke(nameof(beginGame), 0.1f);
        }
    }

    private void beginGame()
    {
        OnBeginGame?.Invoke();
    }

    public override void OnStopClient()
    {
        clientDisconnect();
    }

    public override void OnStopServer()
    {
        serverDisconnect();
    }

    #region DisconectMatch
    public void DisconnectGame()
    {
        cmdDisconnectGame();
        UILobby.instance.disableSearchCanvas();

        if (hasAuthority)
            OnDisconnectGame?.Invoke();
        SceneManager.UnloadSceneAsync(2);
    }

    [Command]
    private void cmdDisconnectGame()
    {
        serverDisconnect();
    }

    private void serverDisconnect()
    {
        Debug.Log("disconnect");
        RoomList.instance.Rooms.Find(room => room.RoomId == RoomID).DisconnectPlayer(this);
        NetworkServer.Destroy(gameObject);
        InGame = false;
        rpcDisconnectGame();
    }

    [ClientRpc]
    private void rpcDisconnectGame()
    {
        clientDisconnect();
    }

    private void clientDisconnect()
    {

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
