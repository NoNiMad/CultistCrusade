using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour {
    public float speed;

    NavMeshAgent agent;
    Rigidbody rb;
    Animator charAnimator;

	void Start () {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        charAnimator = GetComponentInChildren<Animator>();
	}
	
	void Update () {
        Vector3 velocity;

        float horizontalValue = Input.GetAxis("Horizontal");
        float verticalValue = Input.GetAxis("Vertical");

        Transform camTransform = Camera.main.transform;
        Vector3 forward = new Vector3(camTransform.forward.x, 0, camTransform.forward.z).normalized;
        Vector3 right = new Vector3(camTransform.right.x, 0, camTransform.right.z).normalized;
        Vector3 dir = horizontalValue * right + verticalValue * forward;
        transform.LookAt(transform.position + dir);
        velocity = dir * Time.deltaTime * speed;
        agent.Move(velocity);
        //rb.velocity = velocity;
        //Debug.Log(velocity.magnitude);
        if (charAnimator != null)
            charAnimator.SetFloat("Speed", (dir * speed).magnitude);
        rb.angularVelocity = Vector3.zero;
    }
}
