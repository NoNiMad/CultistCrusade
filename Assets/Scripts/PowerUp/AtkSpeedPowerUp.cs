using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkSpeedPowerUp : PowerUp
{
	private float maxReloadBoost = 0.8f;

	public float minionAtkSpeed = 0.1f;
	
	private void Start()
	{
		gameObject.GetComponent<Renderer>().material.color = Color.yellow;
		effect = BoostAtkSpeed;
	}

	private void BoostAtkSpeed()
	{
		if ( BasicAttack.reloadModifier < maxReloadBoost)
			BasicAttack.reloadModifier += minionAtkSpeed;
	}
}
