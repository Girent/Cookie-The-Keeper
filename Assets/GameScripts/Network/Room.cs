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
    private float warmupStageTime = 60f;
    private const float cupStageTime = 20f;
    private const float gameStageTime = 120f;

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
            roomList.StopCoroutine(WarmupTimer());
            warmupStageTime = 5f;
            roomList.StartCoroutine(WarmupTimer());
        }
    }

    public void DisconnectPlayer(GameObject player)
    {
        int playerIndex = players.IndexOf(player);

        players.RemoveAt(playerIndex);

        IsFull = false;

        if (players.Count <= 0)
        {
            Scene scene = SceneManager.GetSceneAt(roomList.Rooms.IndexOf(this) + 1);
            SceneManager.UnloadSceneAsync(scene);

            roomList.Rooms.Remove(this);
        }
        if (players.Count == 1)
        {
            players[0].GetComponent<InGameUi>().WinRps();
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
                players[i].GetComponent<PlayerCombat>().EndWarmup();
                players[i].GetComponent<InGameUi>().StartCupStageRps(cupStageTime);
            }
        InMatch = true;
    }

    private void endCupStage()
    {
        foreach(GameObject player in players)
        {
            CupSpawner cupSpawner = player.GetComponent<CupSpawner>();

            cupSpawner.ForcedSpawn();

            player.GetComponent<InGameUi>().StartGameStageRps(gameStageTime);
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
        yield return new WaitForSeconds(gameStageTime);
        endGameStage();
    }
}
