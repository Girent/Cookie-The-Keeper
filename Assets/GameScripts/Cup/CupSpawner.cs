using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CupSpawner : NetworkBehaviour
{
    [SerializeField] private float setCupRange = 1.5f;
    [SerializeField] private GameObject cupPrefab;
    [SerializeField] private GameObject cupSpawnPoint;
    [SerializeField] private GameObject showBuildPointButton;
    [SerializeField] private GameObject buildButton;
    [SerializeField] private UIJoystick joystickInput;

    private NetworkPlayer networkPlayer;

    private void Start()
    {
        networkPlayer = GetComponent<NetworkPlayer>();
        networkPlayer.OnBeginGame += beginGame;
    }

    private void beginGame()
    {
        cupSpawnPoint.SetActive(false);
        buildButton.SetActive(true);
    }

    private void FixedUpdate()
    {
        cupSpawnPoint.transform.localPosition = joystickInput.GetCurrentDirection() * setCupRange;
    }

    public void ShowBuildingMode()
    {
        cupSpawnPoint.SetActive(true);
        showBuildPointButton.SetActive(true);
        buildButton.SetActive(false);
    }

    [Command]
    public void CmdSpawnCup()
    {
        showBuildPointButton.SetActive(false);
        cupSpawnPoint.SetActive(false);
        spawnCup();
    }

    [Server]
    private void spawnCup()
    {
        GameObject cupObject = Instantiate(cupPrefab, cupSpawnPoint.transform.position, Quaternion.identity);
        cupObject.GetComponent<Cup>().IdMaster = netId;
        NetworkServer.Spawn(cupObject);
    }

    private void OnDestroy()
    {
        networkPlayer.OnBeginGame -= beginGame;
    }

    private void OnDisable()
    {
        networkPlayer.OnBeginGame -= beginGame;
    }
}