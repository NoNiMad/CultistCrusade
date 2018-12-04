using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealSpell : Spell {
    public int amount;
    public float range;

    public override void Cast()
    {
        if (!CanCast()) return;
        base.Cast();
        Collider[] colliders = Physics.OverlapSphere(transform.position, range, LayerMask.GetMask("Minion"));
        foreach (Collider col in colliders)
        {
            col.GetComponent<HealthSystem>().Damage(-amount);
        }
        GetComponent<HealthSystem>().Damage(-amount);
    }
}
