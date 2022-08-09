using UnityEngine;
using Mirror;
using System;

[System.Serializable]
public class SyncListRooms : SyncList<Room> { }

[System.Serializable]
public class SyncListString : SyncList<String> { }

public class RoomList : NetworkBehaviour
{
    public static RoomList instance;

    public SyncList<Room> rooms = new SyncList<Room>();
    public SyncListString roomIDs = new SyncListString();

    private void Awake()
    {
        instance = this;
    }

    public bool HostGame(string roomId, GameObject player, bool publicRoom, out int playerIndex)
    {
        if (!roomIDs.Contains(roomId))
        {
            roomIDs.Add(roomId);
            Room room = new Room(roomId, player);
            room.publicRoom = publicRoom;
            rooms.Add(room);
            NetworkPlayer networkPlayer = player.GetComponent<NetworkPlayer>();
            networkPlayer.currentRoom = room;
            playerIndex = 1;

            StartCoroutine(room.WarmupTimer());
            return true;
        }
        else
        {
            Debug.Log("Id already exists");
            playerIndex = -1;
            return false;
        }
    }

    public bool SearchGame (GameObject player, out string roomId)
    {
        roomId = String.Empty;

        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i].publicRoom && !rooms[i].roomFull && !rooms[i].inMatch)
            {
                roomId = rooms[i].roomId;
                if (roomIDs.Contains(roomId))
                {
                    rooms[i].JoinRoom(player);
                    //JoinGame(roomId, player);
                    return true;
                }
            }
        }

        return false;
    }
}