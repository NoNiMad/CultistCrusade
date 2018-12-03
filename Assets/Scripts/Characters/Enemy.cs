using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Targetable {    
    BasicAttack basicAttack;
    SeismicAreaAttack seismicAttack;
    TargetTracker tracker;
	public float delay;
    float elapsed;

	void Start ()
    {
        tracker = GetComponent<TargetTracker>();
        basicAttack = GetComponent<BasicAttack>();
        seismicAttack = GetComponent<SeismicAreaAttack>();
	}
	
	void Update ()
    {
        elapsed += Time.deltaTime;
        if (elapsed >= delay && seismicAttack.CanExecute() && tracker.GetAllTargets().Count > 4)
        {
            elapsed = 0;
            seismicAttack.Execute();
            delay = basicAttack.reloadTime;
        }
        else if (elapsed >= delay && basicAttack.CanExecute() && tracker.GetAllTargets().Count > 0)
        {
            elapsed = 0;
            basicAttack.Execute();
            delay = basicAttack.reloadTime;
        }
	}

    public override EntitySide GetSide()
    {
        return EntitySide.ENNEMY;
    }
}
