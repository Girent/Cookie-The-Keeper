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

    [SerializeField] private TimerView timerView;

    [SerializeField] private GameObject endGamePanel;
    [SerializeField] private GameObject winUi;
    [SerializeField] private GameObject buildButton;

    [SerializeField] private PlayerSounds playerSounds;

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

    [TargetRpc]
    public void StartCupStageRps(float time)
    {
        startCupStage(time);
        playerSounds.WarmUpEndSound();
    }

    private void startCupStage(float time)
    {
        buildButton.SetActive(true);
        timerView.StartTimer(time, "Put the glass down");
    }

    [TargetRpc]
    public void StartGameStageRps(float time)
    {
        startGameStage(time);
    }

    private void startGameStage(float time)
    {
        timerView.StartTimer(time, "End of battle in");
    }

    private void endGameUi()
    {
        endGamePanel.SetActive(true);
        timerView.gameObject.SetActive(false);
        playerSounds.EndGameSound();
    }

    [TargetRpc]
    public void ToLobbyRps()
    {
        ToLobby();
    }

    public void ToLobby()
    {
        disableCanvas();
        networkPlayer.DisconnectGame();
        endGamePanel.SetActive(false);
        winUi.SetActive(false);
    }

    [TargetRpc]
    public void WinRps()
    {
        Win();
    }

    public void Win()
    {
        winUi.SetActive(true);
        timerView.gameObject.SetActive(false);
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

        timerView.gameObject.SetActive(true);
        timerView.StartTimer(60, "Game start in");

        foreach (var item in playerCanvas)
        {
            item.enabled = true;
        }
    }

    
}
