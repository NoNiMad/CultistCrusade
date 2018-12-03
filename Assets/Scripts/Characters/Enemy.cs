using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Targetable {    
    BasicAttack basicAttack;
    SeismicAreaAttack seismicAttack;
    NavMeshAgent navMeshAgent;
    TargetTracker tracker;
    Animator animator;
    public float delayToDestroy = 60;
    public GameObject cemetery;
	public float delay;
    float elapsed;

	void Start ()
    {
        tracker = GetComponent<TargetTracker>();
        basicAttack = GetComponent<BasicAttack>();
        seismicAttack = GetComponent<SeismicAreaAttack>();
        animator = GetComponentInChildren<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
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
        if (this.animator != null && navMeshAgent != null)
            this.animator.SetFloat("Speed", (this.navMeshAgent.velocity.magnitude / this.navMeshAgent.speed));
	}

    public override EntitySide GetSide()
    {
        return EntitySide.ENNEMY;
    }
}
