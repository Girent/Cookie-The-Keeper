using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SetCup : NetworkBehaviour
{
    private const float setCupRange = 1.5f;

    [SerializeField] private GameObject cup;
    [SerializeField] private GameObject cupTemplate;
    [SerializeField] private GameObject buildButton;
    [SerializeField] private GameObject showBuildButton;

    [SerializeField] private UIJoystick joystickInput;
    private NetworkPlayer networkPlayer;

    private void Start()
    {
        networkPlayer = GetComponent<NetworkPlayer>();
        networkPlayer.OnBeginGame += beginGame;
    }

    private void beginGame()
    {
        cupTemplate.SetActive(false);
        showBuildButton.SetActive(true);
    }

    private void FixedUpdate()
    {
        cupTemplate.transform.localPosition = joystickInput.GetCurrentDirection() * setCupRange;
    }

    public void ShowBuildingMode()
    {
        cupTemplate.SetActive(true);
        buildButton.SetActive(true);
        showBuildButton.SetActive(false);
    }

    [Command]
    public void CmdSpawnCup()
    {
        buildButton.SetActive(false);
        cupTemplate.SetActive(false);
        spawnCup();
    }

    [Server]
    private void spawnCup()
    {
        GameObject cupObject = Instantiate(cup, cupTemplate.transform.position, Quaternion.identity);
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