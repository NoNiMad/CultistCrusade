using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPowerUp : PowerUp
{
	public MinionSpawner target;
	
	public int minionHealth = 20;
	
	void Start () {
		gameObject.GetComponent<Renderer>().material.color = Color.red;
		effect = BoostMinionHealth;
	}
	
	private void BoostMinionHealth()
	{
		MinionSpawner TypedTarget = target.GetComponent<MinionSpawner>();
		TypedTarget.bonusLife += minionHealth;
	}
}
