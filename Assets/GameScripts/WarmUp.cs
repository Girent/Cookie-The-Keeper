using Mirror;
using TMPro;
using UnityEngine;

public class WarmUp : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI displayStatus;
    [SyncVar(hook = nameof(syncIsEndWarmup))] public bool isOver;

    [Server]
    public void IsEndWarmup(bool isOver)
    {
        this.isOver = isOver;
    }

    private void syncIsEndWarmup(bool oldVal, bool newVal)
    {
        if (newVal)
            displayStatus.text = "Матч начался!!";
    }
}