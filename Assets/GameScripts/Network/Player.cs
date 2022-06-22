using UnityEngine;
using Mirror;

[RequireComponent(typeof(NetworkMatch))]

public class Player : NetworkBehaviour
{
    public static Player localPlayer;

    [SyncVar] public string matchId;

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

    public void createRoom()
    {
        string matchId = Extensions.GetRandomMatchID();

        cmdCreateRoom(matchId);
    }

    [Command]
    private void cmdCreateRoom(string _matchId)
    {
        matchId = _matchId;
        if (RoomList.instance.HostGame(_matchId, gameObject))
        {
            networkMatch.matchId = _matchId.ToGuid();
            TargetHostGame(true, _matchId);
            Debug.Log($"<color = green>Room created in ID: {_matchId}</color>");
        }
        else
        {
            TargetHostGame(false, _matchId);
            Debug.Log($"<color = red>Created room error</color>");
        }
    }

    [TargetRpc]
    private void TargetHostGame (bool success, string matchId)
    {
        Debug.Log($"MatchID: {this.matchId}--{matchId}");
        UILobby.instance.HostSuccess(success);
    }
}
