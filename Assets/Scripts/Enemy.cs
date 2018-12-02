using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Targetable {    
    Attack mainAttack;
    TargetTracker tracker;

	void Start()
    {
        mainAttack = GetComponent<Attack>();
        tracker = GetComponent<TargetTracker>();
	}
	
	void Update ()
    {
        if (tracker.HasTargets() && mainAttack.CanExecute())
        {
            mainAttack.Execute();
        }
	}

    public override EntitySide GetSide()
    {
        return EntitySide.ENNEMY;
    }
}
