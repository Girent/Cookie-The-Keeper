using Mirror;
using TMPro;
using UnityEngine;

public class PlayerUi : NetworkBehaviour
{
    [SerializeField] private TextMeshPro nameField;
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private TextMeshProUGUI displayTime;

    [SyncVar(hook = nameof(syncNick))] public string PlayerName;

    private void Awake()
    {
        Application.targetFrameRate = 30;
    }

    public void OnSetNickname()
    {
        if (isServer)
        {
            SetNickname(nameInput.text);
        }
        else
        {
            CmdSetNickname(nameInput.text);
            nameField.text = PlayerName;
        }
    }

    private void syncNick (string oldValue, string newValue)
    {
        nameField.text = newValue;
    }


    [Command]
    public void CmdSetNickname (string nickname)
    {
        SetNickname(nickname);
    }

    [Server]
    public void SetNickname(string nickname)
    {
        PlayerName = nickname;
    }
}
