using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;

public class Enemy : Targetable {    
    BasicAttack basicAttack;
    SeismicAreaAttack seismicAttack;
    NavMeshAgent navMeshAgent;
    TargetTracker tracker;
    Animator animator;
    public float delayToDestroy = 60;
    public GameObject cemetery;
    public bool mustDropPowerUp;

    public GameObject healthPU;
    public GameObject AtkSpeedPU;
    public GameObject spawnPU;
    
    private static Random rnd = new Random();

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
        if (seismicAttack != null && seismicAttack.CanExecute() && tracker.GetAllTargets().Count > 4)
        {
            seismicAttack.Execute();
        }
        else if (basicAttack.CanExecute() && tracker.GetAllTargets().Count > 0)
        {
            basicAttack.Execute();
        }
        if (this.animator != null && navMeshAgent != null)
            this.animator.SetFloat("Speed", (this.navMeshAgent.velocity.magnitude / this.navMeshAgent.speed));
	}

    public override EntitySide GetSide()
    {
        return EntitySide.ENNEMY;
    }
    
    public override void OnDeath()
    {
        bool haveLoot = rnd.Next(1, 20) == 1;
        if (haveLoot || mustDropPowerUp)
        {
            switch (rnd.Next(1, 3))
            {
                case 1:
                    SpawnPowerUp(PowerUpType.BOOST_MINION_SPAWN);
                    break;
                case 2:
                    SpawnPowerUp(PowerUpType.BOOST_MINION_HEALTH);
                    break;
                case 3:
                    if (BasicAttack.reloadModifier <= AtkSpeedPowerUp.maxReloadBoost)
                        SpawnPowerUp(PowerUpType.BOOST_MINION_ATK_SPEED);
                    else
                        SpawnPowerUp(PowerUpType.BOOST_MINION_SPAWN);
                    break;
            }
        }
        Destroy (this.gameObject);
    }

    private void SpawnPowerUp(PowerUpType type)
    {
        MinionSpawner ms = GameObject.Find("MinionSpawner").GetComponent<MinionSpawner>();
        switch (type)
        {
           case PowerUpType.BOOST_MINION_HEALTH:
               HealthPowerUp P1 = Instantiate(healthPU).GetComponent<HealthPowerUp>();
               P1.target = ms;
               P1.transform.position = this.transform.position;
               break;
           case PowerUpType.BOOST_MINION_SPAWN:
               MinionSpawnPowerUp P2 = Instantiate(spawnPU).GetComponent<MinionSpawnPowerUp>();
               P2.target = ms;
               P2.transform.position = this.transform.position;
               break;
           case PowerUpType.BOOST_MINION_ATK_SPEED:
               GameObject P3 = Instantiate(AtkSpeedPU);
               P3.transform.position = this.transform.position;
               break;
        }

    }
}
