using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class UILobby : MonoBehaviour
{
    public static UILobby instance;


    [SerializeField] private Canvas searchCanvas;

    private void Start()
    {
        instance = this;
    }

    public void SearchGame ()
    {
        searchCanvas.enabled = true;

        SceneManager.LoadScene(2, LoadSceneMode.Additive);

        PosMessage m = new PosMessage() { vector2 = Vector2.zero };
        GameObject.FindGameObjectWithTag("Networking").GetComponent<Networking>().connection.Send(m);
    }

    public void disableSearchCanvas()
    {
        searchCanvas.enabled = false;
    }
}
