using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayer : MonoBehaviour
{
    [SerializeField]
    private Text text;

    private NetworkPlayer player;
   public void SetPlayer (NetworkPlayer player)
    {
        this.player = player;
        text.text = "Pidr " + player.PlayerIndex.ToString();
    }
}
