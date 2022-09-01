using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(NetworkMatch))]

public class NetworkPlayer : NetworkBehaviour
{
    [SyncVar] public string RoomID;

    [SyncVar] public Room CurrentRoom;

    public bool InGame = false;

    public static NetworkPlayer localPlayer;
    private NetworkMatch networkMatch;

    public Action OnBeginGame;
    public Action OnStartGame;
    public Action OnDisconnectGame;

    private GameObject[] spawnPoints;

    void Awake()
    {
        networkMatch = GetComponent<NetworkMatch>();
    }

    public override void OnStartClient()
    {
        if (hasAuthority)
        {
            localPlayer = this;
        }
    }

    public override void OnStopClient()
    {
        clientDisconnect();
    }

    public override void OnStopServer()
    {
        serverDisconnect();
    }

    #region CreateGame

    public void CreateRoom(bool publicMatch = true)
    {
        string matchId = Extensions.GetRandomMatchID();

        cmdCreateRoom(matchId, publicMatch);
        BeginGame();
    }

    [Command]
    private void cmdCreateRoom(string _matchId, bool IsPublicMatch)
    {
        RoomID = _matchId;
        if (RoomList.instance.HostGame(_matchId, gameObject, IsPublicMatch))
        {
            networkMatch.matchId = _matchId.ToGuid();
            TargetHostGame( _matchId);
        }
        else
        {
            TargetHostGame( _matchId);
        }
    }

    [TargetRpc]
    private void TargetHostGame (string matchId)
    {
        RoomID = matchId;
    }

    #endregion

    #region SearchGame
    public void SearchGame ()
    {
        cmdSearchGame();
    }

    [Command]
    private void cmdSearchGame()
    {
        if (RoomList.instance.SearchGame(gameObject, out RoomID))
        {
            networkMatch.matchId = RoomID.ToGuid();
            targetSearchGame(true, RoomID);
        }
        else
        {
            targetSearchGame(false, RoomID);
        }
    }

    [TargetRpc]
    private void targetSearchGame(bool success, string matchId)
    {
        RoomID = matchId;
        UILobby.instance.SearchSuccess(success, matchId);
        BeginGame();
    }
    #endregion

    #region BeginGame

    public void MoveToRoomScene(List<GameObject> players)
    {
        targetMoveToRoomScene(players);
    }

    [TargetRpc]
    private void targetMoveToRoomScene(List<GameObject> players)
    {
        Scene sceneToLoad = SceneManager.GetSceneByBuildIndex(2);

        foreach (var player in players)
        {

            if (gameObject != player)
            {
                SceneManager.MoveGameObjectToScene(player, sceneToLoad);
            }
        }
    }

    public void BeginGame()
    {
        cmdBeginGame();
    }

    [Command]
    private void cmdBeginGame()
    {
        RoomList.instance.Rooms.Find(room => room.RoomId == RoomID).EnterRoom();
    }

    public void TargetBeginGame()
    {
        targetBeginGame();
    }

    [TargetRpc]
    private void targetBeginGame()
    {
        OnBeginGame?.Invoke();

        SceneManager.LoadScene(2, LoadSceneMode.Additive);

        Scene sceneToLoad = SceneManager.GetSceneByBuildIndex(2);
        SceneManager.MoveGameObjectToScene(gameObject, sceneToLoad);
    }

    #endregion

    #region DisconectMatch
    public void DisconnectGame ()
    {
        cmdDisconnectGame();
        UILobby.instance.disableSearchCanvas();

        Scene sceneToLobby = SceneManager.GetSceneByBuildIndex(1);

        SceneManager.MoveGameObjectToScene(gameObject, sceneToLobby);
        if(hasAuthority)
            OnDisconnectGame?.Invoke();
        SceneManager.UnloadSceneAsync(2);
    }

    [Command]
    private void cmdDisconnectGame ()
    {
        serverDisconnect();
    }

    private void serverDisconnect ()
    {
        RoomList.instance.Rooms.Find(room => room.RoomId == RoomID).DisconnectPlayer(this);
        networkMatch.matchId = string.Empty.ToGuid();
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