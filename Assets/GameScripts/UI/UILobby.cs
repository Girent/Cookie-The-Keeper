using System.Collections;
using UnityEngine;

public class UILobby : MonoBehaviour
{
    public static UILobby instance;


    [SerializeField] private Canvas searchCanvas;

    private bool searching = false;

    private void Start()
    {
        instance = this;
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
            searching = false;
            searchCanvas.enabled = false;
        }
        else
        {
            searching = false;
            searchCanvas.enabled = false;
            NetworkPlayer.localPlayer.CreateRoom();
        }
       
    }

    public void disableSearchCanvas()
    {
        searchCanvas.enabled = false;
        searching = false;
    }
}
