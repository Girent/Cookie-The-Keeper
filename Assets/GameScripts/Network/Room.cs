using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Room
{
    public int maxPlayers = 2;

    private float warmupTime = 10f;

    public string roomId { get; private set; }

    public bool publicRoom;

    public bool inMatch;

    public bool roomFull;

    public List<GameObject> players = new List<GameObject>();

    public Room(string roomId, GameObject player)
    {
        this.roomId = roomId;
        players.Add(player);
    }

    public Room()
    {
        
    }

    private void StartGame ()
    {
        inMatch = true;
        foreach (GameObject player in players)
        {
            player.GetComponent<WarmUp>().UpdateLocalWarmupStatus(true);
        }
    }

    

    public IEnumerator WarmupTimer()
    {
        yield return new WaitForSeconds(warmupTime);
        StartGame();
    }

}