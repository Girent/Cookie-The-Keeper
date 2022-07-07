using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(NetworkMatch))]

public class NetworkPlayer : NetworkBehaviour
{
    public Scene scene;

    public static NetworkPlayer localPlayer;

    private NetworkMatch networkMatch;

    [SyncVar] public int PlayerIndex;

    [SyncVar] public string RoomID;

    [SyncVar] public Room currentRoom;


    void Awake()
    {
        networkMatch = GetComponent<NetworkMatch>();
    }

    public override void OnStartClient()
    {
        if (isLocalPlayer)
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
    }

    [Command]
    private void cmdCreateRoom(string _matchId, bool publicMatch)
    {
        RoomID = _matchId;
        if (RoomList.instance.HostGame(_matchId, gameObject, publicMatch, out PlayerIndex))
        {
            networkMatch.matchId = _matchId.ToGuid();
            TargetHostGame(true, _matchId);
        }
        else
        {
            TargetHostGame(false, _matchId);
        }
    }

    [TargetRpc]
    private void TargetHostGame (bool success, string matchId)
    {
        RoomID = matchId;
        UILobby.instance.HostSuccess(success, matchId);
    }

    #endregion

    #region JoinGame

    public void JoinRoom(string inputMatchId)
    {
        cmdJoinRoom(inputMatchId);
    }

    [Command]
    private void cmdJoinRoom(string roomId)
    {
        RoomID = roomId;
        if (RoomList.instance.JoinGame(roomId, gameObject, out PlayerIndex))
        {
            networkMatch.matchId = roomId.ToGuid();
            TargetJoinGame(true, roomId);
        }
        else
        {
            TargetJoinGame(false, roomId);
        }
    }

    [TargetRpc]
    private void TargetJoinGame(bool success, string matchId)
    {
        UILobby.instance.JoinSuccess(success, matchId);
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

    public void StartGame()
    {
        TargetBeginGame();
    }

    [TargetRpc]
    private void TargetBeginGame()
    {
        
        SceneManager.LoadScene(2, LoadSceneMode.Additive);
    }

    #endregion

    #region Disconect match
    public void DisconnectGame ()
    {
        cmdDisconnectGame();
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