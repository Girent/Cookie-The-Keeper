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

    public SyncList<Room> Rooms = new SyncList<Room>();
    public SyncListString RoomIDs = new SyncListString();

    private void Awake()
    {
        instance = this;
    }

    public bool HostGame(string roomId, GameObject player, bool IsPublicRoom)
    {
        if (!RoomIDs.Contains(roomId))
        {
            RoomIDs.Add(roomId);
            Room room = new Room(roomId, player);
            room.IsPublicRoom = IsPublicRoom;
            Rooms.Add(room);
            NetworkPlayer networkPlayer = player.GetComponent<NetworkPlayer>();
            networkPlayer.CurrentRoom = room;

            StartCoroutine(room.WarmupTimer());
            return true;
        }
        else
        {
            Debug.Log("Id already exists");
            return false;
        }
    }

    public bool SearchGame (GameObject player, out string roomId)
    {
        roomId = String.Empty;

        for (int i = 0; i < Rooms.Count; i++)
        {
            if (Rooms[i].IsPublicRoom && !Rooms[i].RoomFull && !Rooms[i].InMatch)
            {
                roomId = Rooms[i].RoomId;
                if (RoomIDs.Contains(roomId))
                {
                    Rooms[i].JoinRoom(player);
                    return true;
                }
            }
        }

        return false;
    }
}