using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CupSpawner : NetworkBehaviour
{
    [SerializeField] private float setCupRange = 1.5f;

    [SerializeField] private GameObject cupPrefab;
    [SerializeField] private GameObject cupSpawnPoint;

    [SerializeField] private UIJoystick joystickInput;

    [SerializeField] private GameObject buildButton;
    [SerializeField] private GameObject showBuildPointButton;
    [SerializeField] private GameObject directionUI;

    private GameObject cupObject;

    private SpriteRenderer spriteRendererSpawnPoint;
    private NetworkPlayer networkPlayer;

    [SyncVar] private bool isSpawn = false;

    private void Start()
    {
        networkPlayer = GetComponent<NetworkPlayer>();
        networkPlayer.OnBeginGame += beginGame;
        spriteRendererSpawnPoint = cupSpawnPoint.GetComponent<SpriteRenderer>();
    }

    private void beginGame()
    {
        cupSpawnPoint.SetActive(false);
        showBuildPointButton.SetActive(true);
    }

    private void FixedUpdate()
    {
        cupSpawnPoint.transform.localPosition = joystickInput.GetCurrentDirection() * setCupRange;
    }


    public void ShowBuildingMode()
    {
        cupSpawnPoint.SetActive(true);
        buildButton.SetActive(true);
        showBuildPointButton.SetActive(false);
    }

    private void setColor()
    {
        spriteRendererSpawnPoint.color = Color.green;
    }

    [ClientRpc]
    public void ForcedSpawn()
    {
        if (!isSpawn)
        {
            ShowBuildingMode();
            SpawnCup(true);
            buildButton.SetActive(false);
            cupSpawnPoint.SetActive(false);
        }
    }

    [Client]
    public void SpawnCup(bool forceSpawn = false)
    {
        RaycastHit2D hit2D = Physics2D.Raycast(cupSpawnPoint.transform.position, cupSpawnPoint.transform.TransformDirection(Vector2.down), 1f);

        if (hit2D && forceSpawn == false)
        {
            spriteRendererSpawnPoint.color = Color.red;
            Invoke(nameof(setColor), 0.2f);
        }
        else
        {
            spriteRendererSpawnPoint.color = Color.blue;
            buildButton.SetActive(false);
            cupSpawnPoint.SetActive(false);

            cmdSpawnCup(cupSpawnPoint.transform.position);
            directionUI.SetActive(true);
        }
    }

    public Transform getSpawnPointPosition()
    {
        return cupSpawnPoint.transform;
    }

    [Command]
    private void cmdSpawnCup(Vector3 spawnPoint)
    {
        serverSpawnCup(spawnPoint);
    }

    [Server]
    private void serverSpawnCup(Vector3 spawnPoint)
    {
        isSpawn = true;
        cupObject = Instantiate(cupPrefab, spawnPoint, Quaternion.identity);
        cupObject.GetComponent<Cup>().IdMaster = netId;
        NetworkServer.Spawn(cupObject);

        Scene scene = SceneManager.GetSceneAt(RoomList.instance.Rooms.IndexOf(networkPlayer.CurrentRoom) + 1);
        SceneManager.MoveGameObjectToScene(cupObject, scene);
    }

    private void OnDestroy()
    {
        networkPlayer.OnBeginGame -= beginGame;
    }

    private void OnDisable()
    {
        directionUI.SetActive(false);
        networkPlayer.OnBeginGame -= beginGame;
    }
}