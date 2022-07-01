using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(NetworkMatch))]

public class NetworkPlayer : NetworkBehaviour
{
    public static NetworkPlayer localPlayer;

    private NetworkMatch networkMatch;

    [SyncVar] public int PlayerIndex;

    [SyncVar] public string MatchID;

    [SyncVar] public Room currentRoom;

    void Awake()
    {
        networkMatch = GetComponent<NetworkMatch>();
    }


    private void Start()
    {
        if (isLocalPlayer)
        {
            localPlayer = this;
        }
        else
        {
            UILobby.instance.SpawnUIPlayer(this);
        }
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
        MatchID = _matchId;
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
        MatchID = matchId;
        UILobby.instance.HostSuccess(success, matchId);
    }

    #endregion

    #region JoinGame

    public void JoinRoom(string inputMatchId)
    {
        cmdJoinRoom(inputMatchId);
    }

    [Command]
    private void cmdJoinRoom(string matchId)
    {
        MatchID = matchId;
        if (RoomList.instance.JoinGame(matchId, gameObject, out PlayerIndex))
        {
            networkMatch.matchId = matchId.ToGuid();
            TargetJoinGame(true, matchId);
        }
        else
        {
            TargetJoinGame(false, matchId);
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
        if (RoomList.instance.SearchGame(gameObject, out PlayerIndex, out MatchID))
        {
            networkMatch.matchId = MatchID.ToGuid();
            targetSearchGame(true, MatchID, PlayerIndex);
        }
        else
        {
            targetSearchGame(false, MatchID, PlayerIndex);
        }
    }

    [TargetRpc]
    private void targetSearchGame(bool success, string matchId, int playerIndex)
    {
        PlayerIndex = playerIndex;
        MatchID = matchId;
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
        RoomList.instance.BeginGame(MatchID);
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
}