using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePowerUpOnStep : MonoBehaviour
{

	private bool isAlreadyStep = false;
	
	// Use this for initialization
	void Start () {
		
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.name == "MasterCultist" && !isAlreadyStep)
		{
			Debug.Log("Power Up");
			// Change Color
			Renderer r = this.GetComponent<Renderer>();
			r.material.SetColor("_test", Color.white);
			isAlreadyStep = true;
		}
	}

	private void Update()
	{
		
	}
}
