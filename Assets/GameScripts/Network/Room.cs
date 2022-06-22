using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


[System.Serializable]
public class SyncListGameObject : SyncList<GameObject> { }

[System.Serializable]
public class Room : NetworkBehaviour
{
    private string matchId;
    private SyncListGameObject players = new SyncListGameObject();

    public Room( string matchId, GameObject player)
    {
        this.matchId = matchId;
        players.Add(player);
    }

    public Room() { }
}
