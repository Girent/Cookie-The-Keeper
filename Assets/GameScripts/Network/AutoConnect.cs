using Mirror;
using UnityEngine;

public class AutoConnect : MonoBehaviour
{

    NetworkManager networkManager;

    void Awake()
    {
        networkManager = GetComponent<NetworkManager>();
    }

    void Start()
    {
        if (!Application.isBatchMode)
        { 
            Debug.Log($"=== Client Build ===");
            networkManager.StartClient();
        }
        else
        {
            Debug.Log($"=== Server Build ===");
        }

    }
}
