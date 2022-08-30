using System;
using UnityEngine;
using Mirror;

public class Health : NetworkBehaviour, IProperty, IHealth
{   
    public Health()
    {
        Amount = 100;
    }

    public const float MaxHealth = 100;
    public const float MinHealth = 0;

    [SerializeField] private UIHealthSlider healthSlider;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject popupDamage;

    [SerializeField][SyncVar(hook = nameof(syncValue))] private float amount;


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

    private void Awake()
    {
        animator = player.GetComponent<NetworkAnimator>();
        networkPlayer = player.GetComponent<NetworkPlayer>();
    }

    #region Debug
    [Command]
    public void Increase(int amount)
    {
        increase(amount);
    }

    [Command]
    public void Decrease(int amount)
    {
        decrease(amount);
    }
    #endregion

    [Server]
    private void decrease(int amount)
    {
        if (Amount - amount < MinHealth)
            this.amount = MinHealth;

        this.amount -= amount;

    }

    [Server]
    private void increase(int amount)
    {
        if (Amount + amount > MaxHealth)
            this.amount = MaxHealth;

        this.amount += amount;
    }

    private void syncValue(float oldValue, float newValue)
    {
        amount = newValue;
        healthSlider.UpdateHealthUi(Amount, MaxHealth);

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

    [Command(requiresAuthority = false)]
    public void ApplyDamage(float amount, uint netId)
    {
        if(this.netId != netId)
        {
            applyDamage(amount);
            GameObject popup = Instantiate(popupDamage, gameObject.transform.position, Quaternion.identity);
            popup.GetComponent<DamagePopup>().SetPopupText(amount);
        }
    }

    [Server]
    private void applyDamage(float amount)
    {
        Amount -= amount;
    }
}