using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : NetworkBehaviour
{
    [SerializeField]
    List<NetworkPlayer> players = new List<NetworkPlayer>();

    public void AddPlayer (NetworkPlayer player)
    {
        players.Add(player);
    }
}
