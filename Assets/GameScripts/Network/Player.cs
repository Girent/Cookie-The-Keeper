using UnityEngine;
using Mirror;


public class Player : NetworkBehaviour
{
    public static Player localPlayer;

    private NetworkMatch networkMatch;

    private void Start()
    {
        if (isLocalPlayer)
        {
            localPlayer = this;

            //maybe only Start method
            networkMatch = GetComponent<NetworkMatch>();
        }
    }

    public void createRoom()
    {
        string matchId = "";
        
        for(int i = 10; i>=0; i++)
        {
            matchId += Random.Range(0, 9);
        }

        cmdCreateRoom(matchId);
    }

    [Command]
    private void cmdCreateRoom(string matchId)
    {
        if (RoomList.instance.HostGame(matchId, gameObject))
        {
            networkMatch.matchId = matchId.ToGuid();
            TargetHostGame(true, matchId);
            Debug.Log($"<color = green>Room created in ID: {matchId}</color>");
        }
        else
        {
            TargetHostGame(false, matchId);
            Debug.Log($"<color = red>Created room error</color>");
        }
    }

    [TargetRpc]
    private void TargetHostGame (bool success, string matchId)
    {
        UILobby.instance.HostSuccess(success);
    }
}
