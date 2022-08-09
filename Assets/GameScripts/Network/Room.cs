using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

[System.Serializable]
public class Room
{
    public int maxPlayers = 5;

    private float warmupTime = 10f;

    public string roomId { get; private set; }

    public bool publicRoom;

    public bool inMatch;

    public bool roomFull;

    private RoomList roomList;

    [SyncVar] public List<GameObject> players = new List<GameObject>();

    public Room(string roomId, GameObject player)
    {
        this.roomId = roomId;
        players.Add(player);
        roomList = RoomList.instance;
    }

    public Room()
    {
        
    }

    public void JoinRoom (GameObject player)
    {
        players.Add(player);
        NetworkPlayer networkPlayer = player.GetComponent<NetworkPlayer>();
        networkPlayer.currentRoom = this;
        if (players.Count == maxPlayers)
        {
            roomFull = true;
            //StopCoroutine(WarmupTimer()); надо придумать как остановить корутину от сюда без наследования от монобихейвора
            //startMatch();
        }
    }

    public void EnterRoom()
    {
        foreach (var collectPlayer in players)
        {
            NetworkPlayer player = collectPlayer.GetComponent<NetworkPlayer>();
            if (player.InGame == false)
            {
                player.InGame = true;
                player.StartGame(players);
            }
        }
    }

    public void startMatch ()
    {
        inMatch = true;
        foreach (GameObject player in players)
        {
            
        }
    }

    public void DisconnectPlayer(NetworkPlayer player)
    {
        player.currentRoom = null;
        int playerIndex = players.IndexOf(player.gameObject);
        players.RemoveAt(playerIndex);
        roomFull = false;
        if (players.Count == 0)
        {
            roomList.rooms.Remove(roomList.rooms.Find(room => room.roomId == roomId));
            roomList.roomIDs.Remove(roomId);
        }
    }
    

    public IEnumerator WarmupTimer()
    {
        yield return new WaitForSeconds(warmupTime);
        startMatch();
    }

}