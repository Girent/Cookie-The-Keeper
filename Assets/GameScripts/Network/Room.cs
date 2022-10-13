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
    private const float warmupStageTime = 6f;
    private const float cupStageTime = 2f;
    private const float gameStageTime = 300f;

    private List<GameObject> players = new List<GameObject>();

    public Room(string roomId, GameObject player)
    {
        roomList = RoomList.instance;
        RoomId = roomId;
        players.Add(player);

        roomList.StartCoroutine(WarmupTimer());
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
            endWarmup();
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
            roomList.StopCoroutine(WarmupTimer());
            roomList.StartCoroutine(GameTimmer());

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

    private void endGameStage()
    {
        foreach (GameObject player in players)
        {
            if (player.GetComponent<Health>().isDead)
                player.GetComponent<InGameUi>().ToLobbyRps();
            else
                player.GetComponent<InGameUi>().WinRps();
        }
    }

    public IEnumerator WarmupTimer()
    {
        yield return new WaitForSeconds(warmupStageTime);
        if(!InMatch)
            endWarmup();
        
    }

    public IEnumerator GameTimmer()
    {
        yield return new WaitForSeconds(cupStageTime);
        endCupStage();
        yield return new WaitForSeconds(10);
        endGameStage();
    }
}
