using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILobby : MonoBehaviour
{
    //��� ����� �� ���������, ����� ������ ��Ҳ��� �ײ
    //����� ��ϲ������ ����, ����� � �������� ���ղ��� �� ������ ���!!!
    public static UILobby instance;

    [SerializeField]
    private InputField joinId;

    [SerializeField]
    private Button joinButton;

    [SerializeField]
    private Button hostButton;

    [SerializeField]
    private Canvas lobbyCanvas;

    private void Start()
    {
        instance = this;
    }

    public void Host()
    {
        joinButton.interactable = false;
        hostButton.interactable = false;
        joinId.interactable = false;

        Player.localPlayer.createRoom();
    }

    public void HostSuccess (bool success)
    {
        if (success)
        {
            lobbyCanvas.enabled = true;
        }
        else
        {
            joinButton.interactable = true;
            hostButton.interactable = true;
            joinId.interactable = true;
        }
    }

    public void Join()
    {
        joinButton.interactable = false;
        hostButton.interactable = false;
        joinId.interactable = false;
    }

    public void JoinSuccess(bool success)
    {
        if (success)
        {

        }
        else
        {
            joinButton.interactable = true;
            hostButton.interactable = true;
            joinId.interactable = true;
        }
    }
}
