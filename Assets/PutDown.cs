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
        GameObject bulletGo = Instantiate(cup, transform.position, Quaternion.identity); //Создаем локальный объект пули на сервере
        NetworkServer.Spawn(bulletGo); //отправляем информацию о сетевом объекте всем игрокам.
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
