using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI; 
using UnityEngine;

public class Minion : Targetable {
    public GameObject recenserObject;
    
    MinionRecenser  recenser;
    Attack mainAttack;
    TargetTracker targetTracker;
    NavMeshAgent navMesh;
    Animator charAnimator;
    

    void Start()
    {
        mainAttack = GetComponent<Attack>();
        targetTracker = GetComponent<TargetTracker>();
        navMesh = GetComponent<NavMeshAgent>();
        charAnimator = GetComponentInChildren<Animator>();
        recenser = recenserObject.GetComponent<MinionRecenser>();
        recenser.AddMe();
    }

    void Update()
    {
        //Debug.Log( Vector3.Project(navMesh.desiredVelocity, transform.forward).magnitude / navMesh.speed);
        if (charAnimator != null)
            charAnimator.SetFloat("Speed", Vector3.Project(navMesh.desiredVelocity, transform.forward).magnitude);
        if (mainAttack.CanExecute() && targetTracker.targets.Count > 0)
        {
            mainAttack.Execute();
        }
    }

    public override EntitySide GetSide()
    {
        return EntitySide.FRIENDLY;
    }
    
    void OnDestroy()
    {
        recenser.RemoveMe();
    }
}
