using UnityEngine;
using Mirror;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;

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

    public List<GameObject> GetPlayersOnRoom (string roomId)
    {
        foreach (var room in rooms)
        {
            if (room.roomId == roomId)
            {
                return room.players;
                
            }
        }
        return null;
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

    public bool JoinGame(string roomId, GameObject player, out int playerIndex)
    {
        playerIndex = - 1;
        if (roomIDs.Contains(roomId))
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                if (rooms[i].roomId == roomId)
                {
                    rooms[i].players.Add(player);
                    NetworkPlayer networkPlayer = player.GetComponent<NetworkPlayer>();
                    networkPlayer.currentRoom = rooms[i];

                    playerIndex = rooms[i].players.Count;
                    if (rooms[i].players.Count == rooms[i].maxPlayers)
                        rooms[i].roomFull = true;
                    break;
                }
            }
            return true;
        }
        else
        {
            Debug.Log("Id does not exist");
            return false;
        }
    }


    public bool SearchGame (GameObject player, out int playerIndex, out string roomId)
    {
        playerIndex= -1;
        roomId = String.Empty;

        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i].publicRoom && !rooms[i].roomFull && !rooms[i].inMatch)
            {
                roomId = rooms[i].roomId;
                if (JoinGame(roomId, player, out playerIndex)){
                    return true;
                }
            }
        }

        return false;
    }

    public void BeginGame (string roomId)
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i].roomId == roomId)
            {
                foreach (var collectPlayer in rooms[i].players)
                {
                    NetworkPlayer player = collectPlayer.GetComponent<NetworkPlayer>();
                    if (player.inGame == false)
                    {
                        player.inGame = true;
                        player.StartGame(rooms[i].players);
                    }
                }
                break;
            }
        }
    }

    public void PlayerDisconnected(NetworkPlayer player, string roomId)
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i].roomId == roomId)
            {
                int playerIndex = rooms[i].players.IndexOf(player.gameObject);
                rooms[i].players.RemoveAt(playerIndex);
                rooms[i].roomFull = false;
                Debug.Log($"Player disconnected {roomId}");

                if (rooms[i].players.Count == 0)
                {
                    rooms.RemoveAt(i);
                    roomIDs.Remove(roomId);
                }
                break;
            }
        }
    }
}

public static class Extensions
{
    public static Guid ToGuid(this string id)
    {
        MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
        byte[] inputBytes = Encoding.Default.GetBytes(id);
        byte[] hashBytes = provider.ComputeHash(inputBytes);

        return new Guid(hashBytes);
    }

    public static string GetRandomMatchID()
    {
        string _id = string.Empty;
        for (int i = 0; i < 5; i++)
        {
            int random = UnityEngine.Random.Range(0, 36);
            if (random < 26)
            {
                _id += (char)(random + 65);
            }
            else
            {
                _id += (random - 26).ToString();
            }
        }
        return _id;
    }

}
