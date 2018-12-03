using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
public class QuakeAttack : Attack {
    public int damage;
    public float jumpForce;
    public float range;

    Rigidbody rb;
    NavMeshAgent agent;
    CapsuleCollider col;

    bool jumping;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        col = GetComponent<CapsuleCollider>();
        jumping = false;
    }

    protected override void Update()
    {
        base.Update();
    }

    public override bool CanExecute()
    {
        if (jumping) return false;
        return base.CanExecute();
    }

    public override void Execute()
    {
        jumping = true;
        agent.enabled = false;
        rb.useGravity = true;
        rb.isKinematic = false;
        col.isTrigger = true;
        rb.velocity = Vector3.up * jumpForce;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (jumping && other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            jumping = false;
            agent.enabled = true;
            rb.useGravity = false;
            rb.isKinematic = true;
            col.isTrigger = false;
            StartReloading();

            foreach(Targetable target in tracker.targets)
            {
                if (Vector3.Distance(transform.position, target.transform.position) < range)
                {
                    HealthSystem hs = target.GetComponent<HealthSystem>();
                    if (hs != null)
                    {
                        hs.Damage(damage);
                    }
                }
            }
        }
    }
}
