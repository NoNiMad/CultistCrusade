using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : Attack {
    public int damage;

    public override void Execute()
    {
        StartReloading();
        Targetable target = tracker.GetRandomTarget();
        if (target != null)
        {
            HealthSystem h = (target as MonoBehaviour).GetComponent<HealthSystem>();
            if (h != null)
            {
                if (h.Damage(damage))
                {
                    tracker.RemoveTarget(target);
                }
            }
        }
    }

    public override float GetReloadTime()
    {
        return 1;
    }
}
