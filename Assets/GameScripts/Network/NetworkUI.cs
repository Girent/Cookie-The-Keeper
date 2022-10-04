using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkUI : MonoBehaviour
{
    private void Start()
    {
        SceneManager.LoadScene(2, LoadSceneMode.Additive);
    }

    public void Spawn()
    {
        PosMessage m = new PosMessage() { vector2 = Vector2.zero };
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Networking>().connection.Send(m);
    }
}
