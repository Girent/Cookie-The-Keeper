using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timmerView;

    [SerializeField] private float time;

    private string textMessage;

    public void StartTimer(float setTime, string textMessage)
    {
        time = setTime;
        this.textMessage = textMessage;
    }

    private void FixedUpdate()
    {
        timmerView.SetText($"{textMessage} : {((int)(time -= Time.deltaTime)).ToString()} sec");
    }
}
