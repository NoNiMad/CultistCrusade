using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : Attack {
    public int damage;

    public override void Execute()
    {
        StartReloading();
        HealthSystem hs = tracker.GetRandomTarget<HealthSystem>();
        if (this.GetCharAnimator())
        {
            this.GetCharAnimator().SetTrigger("Attack");
            this.transform.LookAt(hs.transform);
        }
        if (hs != null)
        {
            if (hs.Damage(damage))
            {
                tracker.RemoveTarget(hs.GetComponent<Targetable>());
            }
        }
    }
}
