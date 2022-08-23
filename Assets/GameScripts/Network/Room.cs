using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

[System.Serializable]
public class Room
{
    public string RoomId { get; private set; }

    public bool IsPublicRoom;

    public bool InMatch;

    public bool RoomFull;

    private float warmupTime = 30f;

    private int maxPlayers = 2;

    private RoomList roomList;

    private List<GameObject> players = new List<GameObject>();

    public Room(string roomId, GameObject player)
    {
        RoomId = roomId;
        players.Add(player);
        roomList = RoomList.instance;
    }

    public Room()
    {
        
    }

    public void JoinRoom(GameObject player)
    {
        players.Add(player);
        NetworkPlayer networkPlayer = player.GetComponent<NetworkPlayer>();
        networkPlayer.CurrentRoom = this;
        if (players.Count == maxPlayers)
        {
            RoomFull = true;
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
                player.TargetBeginGame();
            }
        }
    }

    private void startMatch()
    {
        if(!InMatch)
            for (int i = 0; i < players.Count; i++)
            {
                NetworkPlayer networkPlayer = players[i].GetComponent<NetworkPlayer>();

                networkPlayer.MoveToStartPoint(i);
                networkPlayer.MoveToRoomScene(players);
            }
        InMatch = true;
    }

    public void DisconnectPlayer(NetworkPlayer player)
    {
        player.CurrentRoom = null;
        int playerIndex = players.IndexOf(player.gameObject);
        players.RemoveAt(playerIndex);
        RoomFull = false;
        if (players.Count == 0)
        {
            roomList.Rooms.Remove(roomList.Rooms.Find(room => room.RoomId == RoomId));
            roomList.RoomIDs.Remove(RoomId);
        }
    }
    

    public IEnumerator WarmupTimer()
    {
        yield return new WaitForSeconds(warmupTime);
        startMatch();
    }

}