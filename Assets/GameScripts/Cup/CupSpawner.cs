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
    [SerializeField] private GameObject buildButton;
    [SerializeField] private GameObject showBuildPointButton;
    [SerializeField] private UIJoystick joystickInput;

    private SpriteRenderer spriteRendererSpawnPoint;
    private NetworkPlayer networkPlayer;

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

    public void SpawnCup()
    {
        RaycastHit2D hit2D = Physics2D.Raycast(cupSpawnPoint.transform.position, cupSpawnPoint.transform.TransformDirection(Vector2.down), 1f);

        if (hit2D)
        {
            spriteRendererSpawnPoint.color = Color.red;
            Invoke("setColor", 0.2f);
        }
        else
        {
            spriteRendererSpawnPoint.color = Color.blue;
            buildButton.SetActive(false);
            cupSpawnPoint.SetActive(false);
            cmdSpawnCup();
        }
    }

    [Command]
    private void cmdSpawnCup()
    {
        serverSpawnCup();
    }

    [Server]
    private void serverSpawnCup()
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