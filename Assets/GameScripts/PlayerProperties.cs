using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProperties : NetworkBehaviour
{
    [SerializeField] private int maxPlayerHealth;
    [SerializeField] private int playerDamage;
    [SerializeField] private float inGamePlayerSpeed;
    [SerializeField] private float minSpeed = 0.1f;
    [SerializeField] private GameObject[] andrewsHealthPoints;

    [SyncVar(hook = nameof(syncSetCurrentHealth))] public int CurrentPlayerHealth;

    private void Awake()
    {
        CurrentPlayerHealth = maxPlayerHealth;
    }

    private void syncSetCurrentHealth(int oldValue, int newValue)
    {
        CurrentPlayerHealth = newValue;
    }

    [Command]
    public void CmdGettingDamage(int damageAmount)
    {
        gettingDamage(damageAmount);
    }

    [Server]
    private void gettingDamage(int damageAmount)
    {
        CurrentPlayerHealth -= damageAmount;
    }

    #region Speed
    public void IncreaseSpeed(float speed)
    {   
        inGamePlayerSpeed += speed;
    }

    public void DecreaseSpeed(float speed)
    {
        if (inGamePlayerSpeed < minSpeed)
            inGamePlayerSpeed -= speed;
    }
    #endregion

    private void FixedUpdate()
    {
        for (int i = 0; i < andrewsHealthPoints.Length; i++)
        {
            andrewsHealthPoints[i].SetActive(!(CurrentPlayerHealth - 1 < i));
        }
    }
}