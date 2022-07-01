using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILobby : MonoBehaviour
{
    //Òóò ïèçäà øî òâîğèòüñÿ, ÓÂÀÃÀ ÌÎÆÓÒÜ ÂÈÒ²ÊÒÈ Î×²
    //ÓÂÀÃÀ ÊÎÏ²ŞÂÀÍÍß ÊÎÄÓ, ËŞÄßÌ Ç ×ÓÒËÈÂÎŞ ÏÑÈÕ²ÊÎŞ ÍÅ ×ÈÒÀÒÈ ÊÎÄ!!!
    public static UILobby instance;


    [Header("Host äæîèíã")]
    [SerializeField]
    private InputField joinId;

    [SerializeField]
    List<Selectable> lobbySelectables = new List<Selectable>();

    [SerializeField]
    private Canvas lobbyCanvas;

    [SerializeField]
    private Canvas searchCanvas;

    [Header("Lobby")]
    [SerializeField]
    private Transform uiLobbyPlayers;

    [SerializeField]
    private GameObject uiLobbyPrefab;

    [SerializeField]
    private Text matchIdText;


    [SerializeField]
    private GameObject beginGameButton;

    private bool searching = false;

    private void Start()
    {
        instance = this;
    }

    public void Host()
    {
        lobbySelectables.ForEach(selectable => selectable.interactable = false);
        joinId.interactable = false;

        NetworkPlayer.localPlayer.CreateRoom();
    }

    public void HostSuccess (bool success, string matchId)
    {
        if (success)
        {
            lobbyCanvas.enabled = true;
            beginGameButton.SetActive(true);
            SpawnUIPlayer(NetworkPlayer.localPlayer);
            matchIdText.text = matchId;
        }
        else
        {
            lobbySelectables.ForEach(selectable => selectable.interactable = true);
            joinId.interactable = true;
        }
    }

    public void Join()
    {
        NetworkPlayer.localPlayer.JoinRoom(joinId.text);
        lobbySelectables.ForEach(selectable => selectable.interactable = false);
        joinId.interactable = false;
    }

    public void JoinSuccess(bool success, string matchId)
    {
        if (success)
        {
            lobbyCanvas.enabled = true;
            SpawnUIPlayer(NetworkPlayer.localPlayer);
            matchIdText.text = matchId;
        }
        else
        {
            lobbySelectables.ForEach(selectable => selectable.interactable = true);
            joinId.interactable = true;
        }
    }

    public void SpawnUIPlayer (NetworkPlayer player)
    {
        GameObject newUIPlayer = Instantiate(uiLobbyPrefab, uiLobbyPlayers);
        newUIPlayer.GetComponent<UIPlayer>().SetPlayer(player);
        newUIPlayer.transform.SetSiblingIndex(player.PlayerIndex - 1);
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
       
    }

    public void SearchCancel ()
    {
        searchCanvas.enabled = false;
        searching = false;
        lobbySelectables.ForEach(selectable => selectable.interactable = true);
    }
}
