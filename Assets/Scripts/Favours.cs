using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Favours : MonoBehaviour
{
	public Text text;
	public int maxFavour;
	public int currentFavour { get; private set; }

	// Use this for initialization
	void Start ()
	{
		currentFavour = 0;
		text = GetComponent<Text>();
	}

	public void AddOrRemoveFavour(int favor)
	{
		currentFavour += favor;
		if (currentFavour < 0) currentFavour = 0;
		if (currentFavour > maxFavour) currentFavour = maxFavour;
		text.text = currentFavour.ToString();
	}
}
