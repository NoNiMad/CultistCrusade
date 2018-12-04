using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionSpell : Spell {
    public MinionSpawner spawner;
    public int amount;

    public override void Cast()
    {
        base.Cast();
        for (int i = 0; i < amount; i++)
        {
            spawner.SpawnMinion();
        }
    }
}
