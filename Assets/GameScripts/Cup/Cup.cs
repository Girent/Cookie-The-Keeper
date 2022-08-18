using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Cup : NetworkBehaviour, IHealth
{
    public Cup()
    {
        Amount = MaxHealth;
    }

    public const float MaxHealth = 100;
    public const float MinHealth = 0;
    public float Amount
    {
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
    public uint IdMaster;

    [SerializeField] private GameObject popupDamage;
    [SerializeField][SyncVar(hook = nameof(syncValue))] private float amount;

    private void syncValue(float oldValue, float newValue)
    {
        amount = newValue;
        if (amount <= 0)
            destroyObject();
    }

    private void destroyObject()
    {
        NetworkServer.Destroy(gameObject);
    }

    [Command(requiresAuthority = false)]
    public void ApplyDamage(float amount, uint netId)
    {
        if(IdMaster != netId)
        {
            applyDamage(amount);

            GameObject popup = Instantiate(popupDamage, gameObject.transform.position, Quaternion.identity);
            popup.GetComponent<DamagePopup>().Setup(amount);
        }
    }

    [Server]
    private void applyDamage(float amount)
    {
        Amount -= amount;
    }
}
