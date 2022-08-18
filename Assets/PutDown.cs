using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PutDown : NetworkBehaviour
{
    [SerializeField] private GameObject cup;

    [Command]
    public void CmdSpawnBullet()
    {
        spawnBullet();
    }

    [Server]
    private void spawnBullet()
    {
        GameObject bulletGo = Instantiate(cup, transform.position, Quaternion.identity);
        bulletGo.GetComponent<Cup>().IdMaster = netId;
        NetworkServer.Spawn(bulletGo);
    }
}
