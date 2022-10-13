using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Mirror;
using UnityEngine;

public class RoomList : NetworkBehaviour
{

    public static RoomList instance;

    public List<Room> Rooms = new List<Room>();
    public List<string> RoomIDs = new List<string>();


    void Awake()
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

            return true;
        }
        else
        {
            Debug.Log("Id already exists");
            return false;
        }
    }

    public bool SearchRoom(GameObject player, out string roomId)
    {
        roomId = String.Empty;

        for (int i = 0; i < Rooms.Count; i++)
        {
            roomId = Rooms[i].RoomId;

            if (Rooms[i].IsFull == false && Rooms[i].InMatch != true)
            {
                Rooms[i].EnterRoom(player);
                return true;
            }
        }
        return false;
    }

}