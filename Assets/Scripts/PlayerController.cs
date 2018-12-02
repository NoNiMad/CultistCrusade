using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour {
    public float speed;

    NavMeshAgent agent;
    Rigidbody rb;

	void Start () {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
	}
	
	void Update () {
        float horizontalValue = Input.GetAxis("Horizontal");
        float verticalValue = Input.GetAxis("Vertical");

        Transform camTransform = Camera.main.transform;
        Vector3 forward = new Vector3(camTransform.forward.x, 0, camTransform.forward.z).normalized;
        Vector3 right = new Vector3(camTransform.right.x, 0, camTransform.right.z).normalized;
        Vector3 dir = horizontalValue * right + verticalValue * forward;
        transform.LookAt(transform.position + dir);
        rb.velocity = dir * Time.deltaTime * speed;
        rb.angularVelocity = Vector3.zero;
    }
}
