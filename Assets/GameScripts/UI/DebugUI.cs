using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugUI : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private TextMeshProUGUI textMeshPro;
    
    private void FixedUpdate()
    {
        textMeshPro.text = (playerMovement.speed).ToString();
    }
}
