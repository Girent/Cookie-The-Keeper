using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Room
{
    public string roomId { get; private set; }

    public bool publicRoom;

    public bool inMatch;

    public bool roomFull;

    public int maxPlayers = 2;

    public List<GameObject> players = new List<GameObject>();

    public Room(string roomId, GameObject player)
    {
        this.roomId = roomId;
        players.Add(player);
    }

    public Room()
    {

    }
}