using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUp : MonoBehaviour {

	public Action effect;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.GetComponent<PlayerController>() && effect != null)
		{
			effect();
			Destroy(this.gameObject);
		}
	}
	
	void Update ()
	{
		transform.Rotate(new Vector3(15,30,45) * Time.deltaTime);
	}
}
