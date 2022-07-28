using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

[RequireComponent(typeof(NetworkMatch))]

public class NetworkPlayer : NetworkBehaviour
{
    [SyncVar] public int PlayerIndex;

    [SyncVar] public string RoomID;

    [SyncVar] public Room currentRoom;

    public bool InGame = false;

    public Scene scene;

    public static NetworkPlayer localPlayer;
    private NetworkMatch networkMatch;



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
    private void cmdCreateRoom(string _matchId, bool publicMatch)
    {
        RoomID = _matchId;
        if (RoomList.instance.HostGame(_matchId, gameObject, publicMatch, out PlayerIndex))
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
        if (RoomList.instance.SearchGame(gameObject, out PlayerIndex, out RoomID))
        {
            networkMatch.matchId = RoomID.ToGuid();
            targetSearchGame(true, RoomID, PlayerIndex);
        }
        else
        {
            targetSearchGame(false, RoomID, PlayerIndex);
        }
    }

    [TargetRpc]
    private void targetSearchGame(bool success, string matchId, int playerIndex)
    {
        PlayerIndex = playerIndex;
        RoomID = matchId;
        UILobby.instance.SearchSuccess(success, matchId);
        BeginGame();
    }
    #endregion

    #region BeginGame

    public void BeginGame()
    {
        cmdBeginGame();
    }

    [Command]
    private void cmdBeginGame()
    {
        RoomList.instance.BeginGame(RoomID);
    }

    public void StartGame(List<GameObject> players)
    {
        TargetBeginGame(players);
    }

    [TargetRpc]
    private void TargetBeginGame(List<GameObject> players)
    {
        gameObject.GetComponent<PlayerMovement>().EnablePlayerInterface();

        SceneManager.LoadScene(2, LoadSceneMode.Additive);
        Scene sceneToLoad = SceneManager.GetSceneByBuildIndex(2);
        foreach (GameObject player in players)
        {
            SceneManager.MoveGameObjectToScene(player, sceneToLoad);
        }
    }

    #endregion

    #region DisconectMatch
    public void DisconnectGame ()
    {
        cmdDisconnectGame();
        UILobby.instance.disableSearchCanvas();
        Scene sceneToLobby = SceneManager.GetSceneByBuildIndex(1);

        SceneManager.MoveGameObjectToScene(gameObject, sceneToLobby);
        GetComponent<PlayerMovement>().DisablePlayerInterface();
        SceneManager.UnloadSceneAsync(2);
    }

    [Command]
    private void cmdDisconnectGame ()
    {
        serverDisconnect();
    }

    private void serverDisconnect ()
    {
        RoomList.instance.PlayerDisconnected(this, RoomID);
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
}