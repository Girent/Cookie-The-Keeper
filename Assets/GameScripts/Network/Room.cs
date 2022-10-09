using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Room
{
    public bool IsPublicRoom;
    public bool InMatch;
    public bool IsFull;

    public string RoomId { get; private set; }

    private RoomList roomList;

    private int maxPlayers = 2;
    private float warmupTime = 20f;

    private List<GameObject> players = new List<GameObject>();

    public Room(string roomId, GameObject player)
    {
        roomList = RoomList.instance;
        RoomId = roomId;
        players.Add(player);
    }

    public Room()
    {

    }

    public void EnterRoom(GameObject player)
    {
        players.Add(player);
        NetworkPlayer networkPlayer = player.GetComponent<NetworkPlayer>();
        networkPlayer.CurrentRoom = this;
        networkPlayer.RoomID = RoomId;

        if (players.Count >= maxPlayers)
        {
            IsFull = true;
        }
    }

    public void DisconnectPlayer(NetworkPlayer player)
    {
        int playerIndex = players.IndexOf(player.gameObject);
        players.RemoveAt(playerIndex);

        IsFull = false;
        if (players.Count == 0)
        {
            Scene scene = SceneManager.GetSceneAt(roomList.Rooms.IndexOf(this) + 1);
            SceneManager.UnloadSceneAsync(scene);

            roomList.Rooms.Remove(this);
        }
        if (players.Count == 1)
        {
            players[0].GetComponent<InGameUi>().Win();
            InMatch = true;
        }
    }

    private void endWarmup()
    {
        if (!InMatch)
            for (int i = 0; i < players.Count; i++)
            {
                players[i].GetComponent<NetworkPlayer>().MoveToStartPoint(i);
                players[i].GetComponent<PlayerCombat>().endWarmup();
            }
        InMatch = true;
    }

    private void endCupStage()
    {
        for (int i = 0; i < players.Count; i++)
        {
            CupSpawner cupSpawner = players[i].GetComponent<CupSpawner>();

            cupSpawner.ForcedSpawn();
        }
    }

    public IEnumerator WarmupTimer()
    {
        yield return new WaitForSeconds(warmupTime);
        endWarmup();
        yield return new WaitForSeconds(10);
        endCupStage();
    }
}
