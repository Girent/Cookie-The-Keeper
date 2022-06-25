using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Room
{
    public string matchId;
    public List<GameObject> players = new List<GameObject>();

    public Room(string matchId, GameObject player)
    {
        this.matchId = matchId;
        players.Add(player);
        Debug.Log(players[0] + "Room class");
        Debug.Log(this.matchId + "Room class");
    }

    public Room()
    {

    }
}