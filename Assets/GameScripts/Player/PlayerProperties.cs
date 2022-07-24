using Mirror;
using UnityEngine;

public class PlayerProperties : NetworkBehaviour
{
    [SerializeField] private int maxPlayerHealth;
    public int playerDamage { get; private set; }
    public float attackRange { get; private set; }
    [SerializeField] private float damageReductionCoefficient;
    [SerializeField] private float inGamePlayerSpeed;
    [SerializeField] private float minSpeed = 0.1f;
    [SerializeField] private GameObject[] andrewsHealthPoints;

    [SyncVar(hook = nameof(syncSetCurrentHealth))] public int CurrentPlayerHealth;

    private void Awake()
    {
        attackRange = 1.2f;
        playerDamage = 1;
        CurrentPlayerHealth = maxPlayerHealth;
    }

    private void syncSetCurrentHealth(int oldValue, int newValue)
    {
        CurrentPlayerHealth = newValue;
    }

    [Command]
    public void CmdGettingDamage(int damageAmount)
    {
        GettingDamage(damageAmount);
    }

    [Server]
    public void GettingDamage(int damageAmount)
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