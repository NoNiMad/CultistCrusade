using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnnemyIA : MonoBehaviour {
	public EntitySide toAttack;
	public float detectionRadius;
	public float AbandonRadius;
	public float wander = 3;
	NavMeshAgent navMeshAgent;
	Targetable currentTarget = null;

	// Use this for initialization
	void Start () {
		navMeshAgent = this.GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
		float lastDist = -1;
		Collider[] hitColliders = Physics.OverlapSphere(this.gameObject.transform.position, detectionRadius);
		if (currentTarget != null && (currentTarget.transform.position - this.transform.position).magnitude > AbandonRadius)
		{
			this.navMeshAgent.SetDestination(this.transform.position);
			this.currentTarget = null;
		}

        foreach (Collider collider in hitColliders) {
			Targetable tmp = collider.GetComponent<Targetable>();
			if (tmp != null && tmp.GetSide() == toAttack && (lastDist < 0 || (this.transform.position - tmp.transform.position).magnitude < lastDist))
			{
				currentTarget = tmp;
				lastDist = (this.transform.position - tmp.transform.position).magnitude;
			}
		}
		if (this.currentTarget != null && navMeshAgent != null)
		{
			navMeshAgent.SetDestination(this.currentTarget.transform.position);
		}
		else if (this.currentTarget == null && navMeshAgent != null && navMeshAgent.velocity.magnitude < 0.2)
		{
			Vector2 tmpWander = Random.insideUnitCircle * wander;
			Vector3 tmpWanderTwo = new Vector3(tmpWander.x, 0, tmpWander.y);
			navMeshAgent.SetDestination(tmpWanderTwo + this.transform.position);
		}
	}

	public void resetTarget() {
		this.currentTarget = null;
	}
}
