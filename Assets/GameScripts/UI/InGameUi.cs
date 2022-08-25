using System.Linq;
using Mirror;
using UnityEngine;

public class InGameUi : NetworkBehaviour
{
    [SerializeField] private Canvas[] playerCanvas;
    [SerializeField] private NetworkPlayer networkPlayer;
    [SerializeField] private NetworkAnimator networkAnimator;
    [SerializeField] private Health health;
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private GameObject endGamePanel;
    [SerializeField] private GameObject winUi;
    private GameObject mainCamera;

    private void Awake()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    private void Start()
    {
        if (hasAuthority)
        {
            networkPlayer.OnBeginGame += enableCanvas;
            health.OnPlayerDead += endGameUi;
        }
    }

    private void OnDisable()
    {
        networkPlayer.OnBeginGame -= enableCanvas;
        health.OnPlayerDead -= endGameUi;
    }

    private void OnDestroy()
    {
        networkPlayer.OnBeginGame -= enableCanvas;
        health.OnPlayerDead -= endGameUi;
    }

    private void endGameUi()
    {
        endGamePanel.SetActive(true);
        Invoke("ToLobby", 5f);
    }

    public void ToLobby()
    {
        disableCanvas();
        networkPlayer.DisconnectGame();
        endGamePanel.SetActive(false);
        winUi.SetActive(false);
        networkAnimator.SetTrigger("Live");
    }

    public void Win()
    {
        winUi.SetActive(true);
    }

    private void disableCanvas()
    {
        mainCamera.SetActive(true);
        playerCamera.SetActive(false);
        foreach (var item in playerCanvas)
        {
            item.enabled = false;
        }
    }

    private void enableCanvas()
    {
        mainCamera.SetActive(false);
        playerCamera.SetActive(true);
        foreach (var item in playerCanvas)
        {
            item.enabled = true;
        }
    }
}
