using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI; 
using UnityEngine;

public class Minion : Targetable {
    public float delayToDestroy;
    public AudioClip rushScream;
    
    public GameObject recenserObject;
    public GameObject cemetery;

    MinionRecenser  recenser;
    TargetTracker targetTracker;
    NavMeshAgent navMesh;
    Animator charAnimator;
    AudioSource audioEmitter;
    BasicAttack basicAttack;
    SeismicAreaAttack seismicAttack;
    

    void Start()
    {
        basicAttack = GetComponent<BasicAttack>();
        seismicAttack = GetComponent<SeismicAreaAttack>();
        targetTracker = GetComponent<TargetTracker>();
        navMesh = GetComponent<NavMeshAgent>();
        charAnimator = GetComponentInChildren<Animator>();
        recenser = recenserObject.GetComponent<MinionRecenser>();
        audioEmitter = this.GetComponent<AudioSource>();
        recenser.AddMe();
    }

    void Update()
    {
        if (charAnimator != null)
            charAnimator.SetFloat("Speed", navMesh.velocity.magnitude /  navMesh.speed);
        if (targetTracker.targets.Count > 0)
        {
            if (targetTracker.targets.Count < 8 && basicAttack != null && basicAttack.CanExecute())
                basicAttack.Execute();
            else if (seismicAttack != null && seismicAttack.CanExecute())
                seismicAttack.Execute();
        }
    }

    public override EntitySide GetSide()
    {
        return EntitySide.FRIENDLY;
    }
    
    public override void OnDeath()
    {
        int i = this.deathClips.Length;
        DestroyAfter timeBomb = this.gameObject.AddComponent<DestroyAfter>() as DestroyAfter;

        i = Random.Range(0, i - 1);

        if (charAnimator != null)
            charAnimator.SetTrigger("Death");
        this.GetComponent<HealthSystem>().enabled = false;
        this.GetComponent<TargetTracker>().enabled = false;
        this.GetComponent<Collider>().enabled = false;
        this.GetComponent<NavMeshAgent>().enabled = false;
        this.GetComponent<BasicAttack>().enabled = false;
        this.gameObject.transform.SetParent(cemetery.gameObject.transform);
        if (audioEmitter != null && this.deathClips.Length > 0)
            audioEmitter.PlayOneShot(this.deathClips[i]);
        recenser.RemoveMe();
        this.enabled = false;
        timeBomb.delay = delayToDestroy;
    }
}
