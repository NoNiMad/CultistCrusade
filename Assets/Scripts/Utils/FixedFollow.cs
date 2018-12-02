using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedFollow : MonoBehaviour {
    public Transform toFollow;

    Vector3 offset;

	void Start () {
        offset = transform.position - toFollow.position;
	}
	
	void LateUpdate () {
        transform.position = offset + toFollow.position;
	}
}
