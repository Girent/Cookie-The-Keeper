using UnityEngine;
using Mirror;



[RequireComponent(typeof(NetworkMatch))]

public class Player : NetworkBehaviour
{
    public static Player localPlayer;

    private NetworkMatch networkMatch;

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
    }

    public void CreateRoom()
    {
        string matchId = Extensions.GetRandomMatchID();

        cmdCreateRoom(matchId);
    }

    [Command]
    private void cmdCreateRoom(string _matchId)
    {
        if (RoomList.instance.HostGame(_matchId, gameObject))
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
        UILobby.instance.HostSuccess(success);
    }

    public void JoinRoom(string inputMatchId)
    {
        cmdJoinRoom(inputMatchId);
    }

    [Command]
    private void cmdJoinRoom(string _matchId)
    {
        if (RoomList.instance.JoinGame(_matchId, gameObject))
        {
            networkMatch.matchId = _matchId.ToGuid();
            TargetJoinGame(true, _matchId);
        }
        else
        {
            TargetJoinGame(false, _matchId);
        }
    }

    [TargetRpc]
    private void TargetJoinGame(bool success, string matchId)
    {
        UILobby.instance.HostSuccess(success);
    }
}