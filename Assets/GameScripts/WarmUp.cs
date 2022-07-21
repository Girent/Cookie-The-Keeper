using Mirror;
using TMPro;
using UnityEngine;

public class WarmUp : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI displayStatus;
    [SyncVar(hook = nameof(syncUpdateLocalWarmupStatus))] public bool isOver;

    [Server]
    public void UpdateLocalWarmupStatus(bool isOver)
    {
        this.isOver = isOver;
    }

    private void syncUpdateLocalWarmupStatus(bool oldVal, bool newVal)
    {
        if (newVal)
            displayStatus.text = "Матч начался!!";
    }
}