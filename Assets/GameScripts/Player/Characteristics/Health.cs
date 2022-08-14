using System;
using UnityEngine;
using Mirror;

public class Health : NetworkBehaviour, IProperty 
{   
    public Health()
    {
        Amount = 100;
    }

    public const float MaxHealth = 100;
    public const float MinHealth = 0;

    [SerializeField] private UIHealthSlider healthSlider;
    [SerializeField] private GameObject player;

    private NetworkAnimator animator;
    private NetworkPlayer networkPlayer;

    public Action OnPlayerDead;

    public float Amount{
        get 
        { 
            return amount; 
        }
        private set
        {
            amount = value;
            if (amount > MaxHealth)
                amount = MaxHealth;
        }
    }

    [SerializeField][SyncVar(hook = nameof(syncValue))] private float amount;

    private void Awake()
    {
        animator = player.GetComponent<NetworkAnimator>();
        networkPlayer = player.GetComponent<NetworkPlayer>();
    }

    public void Decrease(int amount)
    {
        if (Amount - amount < MinHealth)
            throw new InvalidOperationException();
        
        Amount -= amount;

    }

    public void Increase(int amount)
    {
        if(Amount + amount > MaxHealth)
            throw new InvalidOperationException();

        Amount += amount;
    }

    private void syncValue(float oldValue, float newValue)
    {
        amount = newValue;
        healthSlider.UpdateHealthUi(amount, MaxHealth);

        if (amount <= 0)
        {
            if (hasAuthority)
                OnPlayerDead?.Invoke();
        }
    }

    [Server]
    public void serverSetMaxHealthAmount()
    {
        Amount = MaxHealth;
    }

    [Server]
    public void ApplyDamage(float amount)
    {
        Amount -= amount;
    }

}