using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Networking : NetworkManager
{
    public static Networking instance;

    public NetworkConnection connection;

    public void OnCreateCharacter(NetworkConnection conn, PosMessage message)
    {
        GameObject go = Instantiate(playerPrefab, message.vector2, Quaternion.identity);
        NetworkPlayer networkPlayer = go.GetComponent<NetworkPlayer>();

        string roomId = "";

        if (!RoomList.instance.SearchRoom(go, out roomId))
        {
            roomId = Extensions.GetRandomMatchID();
            networkPlayer.RoomID = roomId;

            SceneManager.LoadScene(2, LoadSceneMode.Additive);
            RoomList.instance.HostGame(roomId, go, true);

            Scene scene = SceneManager.GetSceneAt(RoomList.instance.Rooms.Count);
            SceneManager.MoveGameObjectToScene(go, scene);
        }
        else
        {
            networkPlayer.RoomID = roomId;
            Scene scene = SceneManager.GetSceneAt(RoomList.instance.Rooms.Count);
            SceneManager.MoveGameObjectToScene(go, scene);
        }

        UILobby.instance.disableSearchCanvas();

        NetworkServer.AddPlayerForConnection((NetworkConnectionToClient)conn, go);
    }

    public override void OnStartServer()
    {
        
        instance = this;
        base.OnStartServer();
        NetworkServer.RegisterHandler<PosMessage>(OnCreateCharacter);
    }

   

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        connection = conn;
    }
}

public struct PosMessage : NetworkMessage
{
    public Vector2 vector2;
}
