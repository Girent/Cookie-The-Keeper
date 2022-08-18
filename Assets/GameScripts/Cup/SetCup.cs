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
    [SerializeField] private GameObject davaiPodimaem;
    [SerializeField] private UIJoystick joystickInput;
    [SerializeField] private GameObject buildButton;
    [SerializeField] private GameObject showBuildButton;
    private NetworkPlayer networkPlayer;

    private void Start()
    {
        networkPlayer = GetComponent<NetworkPlayer>();
        networkPlayer.OnBeginGame += beginGame;
    }

    private void beginGame()
    {
        davaiPodimaem.SetActive(false);
        showBuildButton.SetActive(true);
    }

    private void FixedUpdate()
    {
        davaiPodimaem.transform.localPosition = joystickInput.GetCurrentDirection() * setCupRange;
    }

    public void ShowBuildingMode()
    {
        davaiPodimaem.SetActive(true);
        buildButton.SetActive(true);
        showBuildButton.SetActive(false);
    }

    [Command]
    public void CmdSpawnCup()
    {
        buildButton.SetActive(false);
        davaiPodimaem.SetActive(false);
        spawnCup();
    }

    [Server]
    private void spawnCup()
    {
        GameObject cupObject = Instantiate(cup, davaiPodimaem.transform.position, Quaternion.identity);
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