using System;
using Mirror;
using UnityEngine;

public class PlayerCombat : NetworkBehaviour
{
    public int milkPoints;

    [SerializeField] private float attackRange;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private UIJoystick joystickInput;

    private NetworkAnimator networkAnimator;
    private Damage damage;

    [SyncVar]private bool isAllow = false;

    private void Awake()
    {
        damage = GetComponent<Damage>();
        networkAnimator = GetComponent<NetworkAnimator>();
    }

    public void endWarmup()
    {
        isAllow = true;
    }

    public void Attack ()
    {
        attackPoint.localPosition = joystickInput.GetLastDirection() * attackRange;

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange / 2f, enemyLayers);

        setAttackAmination();

        if(isAllow)
            foreach (var enemy in hitEnemies)
            {
                InflictDamage(enemy.GetComponent<IHealth>(), damage.Amount);
            }
    }

    private void setAttackAmination()
    {
        networkAnimator.animator.SetFloat("AttackVectorHorizontal", joystickInput.GetLastDirection().x);
        networkAnimator.animator.SetFloat("AttackVectorVertical", joystickInput.GetLastDirection().y);
        networkAnimator.SetTrigger("Attack");
    }

    public void InflictDamage(IHealth health, float damage)
    {
        health.ApplyDamage(damage, netId, gameObject);
    }
}