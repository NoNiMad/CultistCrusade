using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionRecenser : MonoBehaviour {
	int howManyAlive = 0;

	public void AddMe() {
		howManyAlive++;
	}

	public void RemoveMe() {
		howManyAlive--;
	}

	public int HowMany() {
		return howManyAlive;
	}

	void Update()
	{
		Debug.Log(howManyAlive + " minions are alive");
	}
}
