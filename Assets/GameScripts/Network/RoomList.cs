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

    public SyncList<Room> rooms = new SyncList<Room>();
    public SyncListString roomIDs = new SyncListString();

    [SerializeField]
    private GameObject turnManagerPrefab;

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
            player.GetComponent<NetworkPlayer>().currentRoom = room;
            playerIndex = 1;
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
                    player.GetComponent<NetworkPlayer>().currentRoom = rooms[i];
                    playerIndex = rooms[i].players.Count;
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
        GameObject newTurnManager = Instantiate(turnManagerPrefab);
        NetworkServer.Spawn(newTurnManager);
        newTurnManager.GetComponent<NetworkMatch>().matchId =  roomId.ToGuid();
        TurnManager turnManager = newTurnManager.GetComponent<TurnManager>(); //Скорей всего мы берём первый менеджер из списка нескольких, и попадаем на не тот что нам нужен
        

        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i].roomId == roomId)
            {
                foreach (var collectPlayer in rooms[i].players)
                {
                    NetworkPlayer player = collectPlayer.GetComponent<NetworkPlayer>();
                    turnManager.AddPlayer(player);
                    player.StartGame();
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
