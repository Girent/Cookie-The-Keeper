using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILobby : MonoBehaviour
{
    //??? ????? ?? ?????????, ????? ?????? ??????? ???
    //????? ?????????? ????, ????? ? ???????? ???????? ?? ?????? ???!!!
    public static UILobby instance;


    [Header("Host ??????")]

    [SerializeField]
    List<Selectable> lobbySelectables = new List<Selectable>();

    [SerializeField]
    private Canvas lobbyCanvas;

    [SerializeField]
    private Canvas searchCanvas;

    [SerializeField]
    private Text matchIdText;


    [SerializeField]
    private GameObject beginGameButton;

    private bool searching = false;

    private void Start()
    {
        instance = this;
    }

    private void Host()
    {
        lobbySelectables.ForEach(selectable => selectable.interactable = false);

        NetworkPlayer.localPlayer.CreateRoom();
    }

    public void HostSuccess (bool success, string matchId)
    {
        if (success)
        {
            lobbyCanvas.enabled = true;
            beginGameButton.SetActive(true);
            matchIdText.text = matchId;
        }
        else
        {
            lobbySelectables.ForEach(selectable => selectable.interactable = true);
        }
    }

    public void JoinSuccess(bool success, string matchId)
    {
        if (success)
        {
            lobbyCanvas.enabled = true;
            beginGameButton.SetActive(false);

            matchIdText.text = matchId;
        }
        else
        {
            lobbySelectables.ForEach(selectable => selectable.interactable = true);
        }
    }

    public void BeginGame ()
    {
        NetworkPlayer.localPlayer.BeginGame();
    }

    public void SearchGame ()
    {
        searchCanvas.enabled = true;
        StartCoroutine(SearchingForGame());
    }

    IEnumerator SearchingForGame ()
    {
        searching = true;
        float currentTime = 1;
        while (searching)
        {
            if (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
            }
            else
            {
                currentTime = 1;
                NetworkPlayer.localPlayer.SearchGame();
            }
            yield return null;
        }
    }

    public void SearchSuccess(bool success, string matchId)
    {
        if (success)
        {
            JoinSuccess(success, matchId);
            searching = false;
            searchCanvas.enabled = false;
        }
        else
        {
            searching = false;
            searchCanvas.enabled = false;
            Host();
        }
       
    }

    public void SearchCancel ()
    {
        searchCanvas.enabled = false;
        searching = false;
        lobbySelectables.ForEach(selectable => selectable.interactable = true);
    }

    public void DisconnectLobby ()
    {
        NetworkPlayer.localPlayer.DisconnectGame();

        lobbyCanvas.enabled = false;
        lobbySelectables.ForEach(selectable => selectable.interactable = true);
        beginGameButton.SetActive(false);
    }
}
