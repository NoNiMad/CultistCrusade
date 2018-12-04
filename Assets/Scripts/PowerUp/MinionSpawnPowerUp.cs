using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionSpawnPowerUp : PowerUp	
{
	public MinionSpawner target;
	public int minionSpawn = 2;
	
	private void Start()
	{
		gameObject.GetComponent<Renderer>().material.color = Color.blue;
		effect = BoostMinionSpawn;
	}

	private void BoostMinionSpawn()
	{
		target.howManyPerGroup+= minionSpawn;
		if (target.maxCultists <= 60)
			target.maxCultists+= minionSpawn;
	}
}
