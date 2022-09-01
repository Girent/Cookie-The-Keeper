using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerWarmupView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timmerView;
    [SerializeField] private NetworkPlayer networkPlayer;
    private bool isOver;
    private float time;
    private const float maxTime = 60f; 
    private void Start()
    {
        networkPlayer.OnStartGame += endWarmup;
        networkPlayer.OnBeginGame += startWarmup;
    }

    private void startWarmup()
    {
        isOver = false;
        timmerView.enabled = true;
        time = maxTime;
    }

    private void endWarmup()
    {
        isOver = true;
        timmerView.enabled = false;
    }

    private void FixedUpdate()
    {
        if (!isOver)
            timmerView.SetText($"Start in: {((int)(time -= Time.deltaTime)).ToString()} sec");
    }
}
