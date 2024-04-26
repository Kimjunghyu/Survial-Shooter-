using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : LivingEntity
{
    public Animator playerAnimator;

    public PlayerMove playerMovement;
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        if (dead)
            return;

        base.OnDamage(damage, hitPoint, hitDirection);
    }
    public override void Die()
    {
        base.Die();
        playerAnimator.SetTrigger("Die");

        playerMovement.enabled = false;
    }
}
