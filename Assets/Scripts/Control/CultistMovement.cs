using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CultistMovement : MonoBehaviour
{

	public int vitesse;
	
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		var fHorizontalValue = Input.GetAxis("Horizontal");
		var fVerticalValue = Input.GetAxis("Vertical");
	    
        transform.Translate(fHorizontalValue*Time.deltaTime*vitesse, 0f,fVerticalValue*Time.deltaTime*vitesse);
	}
}
