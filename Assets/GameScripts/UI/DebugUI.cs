using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugUI : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Health health;
    [SerializeField] private Damage damage;

    [SerializeField] private TextMeshProUGUI speedUI;
    [SerializeField] private TextMeshProUGUI damageUI;
    [SerializeField] private TextMeshProUGUI healthUI;

    private void FixedUpdate()
    {
        speedUI.text = (playerMovement.Speed).ToString();
        damageUI.text = (damage.Amount).ToString();
        healthUI.text = (health.Amount).ToString();
    }
}
