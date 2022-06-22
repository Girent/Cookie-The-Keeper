using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using System.Security.Cryptography;
using System.Text;

[System.Serializable]
public class SyncListRooms : SyncList<Room> { }

[System.Serializable]
public class SyncListString : SyncList<String> { }

public class RoomList : NetworkBehaviour
{
    public static RoomList instance;

    public SyncListRooms rooms = new SyncListRooms();
    public SyncListString matchIDs = new SyncListString();

    private void Awake()
    {
        instance = this;
    }

    public bool HostGame(string matchId, GameObject player)
    {
        if (!matchIDs.Contains(matchId))
        {
            matchIDs.Add(matchId);
            rooms.Add(new Room(matchId, player));
            Debug.Log(matchId);
            return true;
        }
        else
        {
            Debug.Log("Id already exists");
            return false;
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
        Debug.Log($"Random Match ID: {_id}");
        return _id;
    }

}
