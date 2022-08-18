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

    private int maxPlayers = 5;

    private RoomList roomList;

    [SyncVar] public List<GameObject> Players = new List<GameObject>();

    public Room(string roomId, GameObject player)
    {
        RoomId = roomId;
        Players.Add(player);
        roomList = RoomList.instance;
    }

    public Room()
    {
        
    }

    public void JoinRoom(GameObject player)
    {
        Players.Add(player);
        NetworkPlayer networkPlayer = player.GetComponent<NetworkPlayer>();
        networkPlayer.CurrentRoom = this;
        if (Players.Count == maxPlayers)
        {
            RoomFull = true;
            //StopCoroutine(WarmupTimer()); надо придумать как остановить корутину от сюда без наследования от монобихейвора
            //startMatch();
        }
    }

    public void EnterRoom()
    {
        foreach (var collectPlayer in Players)
        {
            NetworkPlayer player = collectPlayer.GetComponent<NetworkPlayer>();
            if (player.InGame == false)
            {
                player.InGame = true;
                player.StartGame(Players);
            }
        }
    }

    public void StartMatch()
    {
        InMatch = true;
        for (int i = 0; i < Players.Count; i++)
        {
            Players[i].GetComponent<NetworkPlayer>().MoveToStartPoint(i);
        }
    }

    public void DisconnectPlayer(NetworkPlayer player)
    {
        player.CurrentRoom = null;
        int playerIndex = Players.IndexOf(player.gameObject);
        Players.RemoveAt(playerIndex);
        RoomFull = false;
        if (Players.Count == 0)
        {
            roomList.Rooms.Remove(roomList.Rooms.Find(room => room.RoomId == RoomId));
            roomList.RoomIDs.Remove(RoomId);
        }
    }
    

    public IEnumerator WarmupTimer()
    {
        yield return new WaitForSeconds(warmupTime);
        StartMatch();
    }

}