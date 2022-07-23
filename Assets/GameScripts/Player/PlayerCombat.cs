using Mirror;
using UnityEngine;

public class PlayerCombat : NetworkBehaviour
{
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] UIJoystick joystickInput;

    private Vector3 attackDirection = new Vector3(1,1,0);

    private PlayerProperties playerProperties;

    private void Awake()
    {
        playerProperties = gameObject.GetComponent<PlayerProperties>();
    }

    private void FixedUpdate()
    {
        if (joystickInput.HorizontalInput() != 0 && joystickInput.VerticallInput() != 0)
            attackDirection = new Vector3(joystickInput.HorizontalInput(), joystickInput.VerticallInput(), 0).normalized * playerProperties.attackRange;
    }

    public void Attack ()
    {
        attackPoint.localPosition = attackDirection;

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, playerProperties.attackRange / 4, enemyLayers);

        foreach (var enemy in hitEnemies)
        {
            damageEnemy(enemy.GetComponent<PlayerProperties>());
        }
    }

    [Command]
    private void damageEnemy (PlayerProperties enemy)
    {
        enemy.GettingDamage(playerProperties.playerDamage);
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

            Gizmos.DrawWireSphere(attackPoint.position, playerProperties.attackRange / 4);
    }
}
