using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spells : MonoBehaviour
{

	public Favours favor;
	
	public void executeSpell1(int costFavor)
	{
		Debug.Log("Try to use Spell 1");
		if (favor.currentFavour - costFavor < 0)
		{
			Debug.Log("Use Spell 1");
			favor.AddOrRemoveFavour(costFavor);
		}
	}
	
	public void executeSpell2(int costFavor)
	{
		Debug.Log("try to use Spell 2");
		if (favor.currentFavour - costFavor < 0)
		{
			Debug.Log("Use Spell 1");
			favor.AddOrRemoveFavour(costFavor);
		}
	}
	
	public void executeSpell3(int costFavor)
	{
		Debug.Log("try to use Spell 3");
		if (favor.currentFavour - costFavor < 0)
		{
			Debug.Log("Use Spell 1");
			favor.AddOrRemoveFavour(costFavor);
		}
	}
	
	public void executeSpell4(int costFavor)
	{
		Debug.Log("try to use Spell 4");
		if (favor.currentFavour - costFavor < 0)
		{
			Debug.Log("Use Spell 1");
			favor.AddOrRemoveFavour(costFavor);
		}
	}
	
	public void executeSpell5(int costFavor)
	{
		Debug.Log("try to use Spell 5");
		if (favor.currentFavour - costFavor < 0)
		{
			Debug.Log("Use Spell 1");
			favor.AddOrRemoveFavour(costFavor);
		}
	}
}
