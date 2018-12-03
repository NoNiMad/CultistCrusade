using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FavourManager : MonoBehaviour {
	public int littleFontSize = 26;
	public Color littleColor;
	public int bigFontSize = 48;
	public Color bigColor;
	public int maxFavours;

	int favours = 0;
	public Text textField;

	public void AddFavours(int n) {
		favours += n;
		if (favours > maxFavours)
			favours = maxFavours;
		else if (favours < 0)
			favours = 0;
	}

	public int HowManyFavours() {
		return favours;
	}

	public void Update() {
		float percentile = (float)favours / maxFavours;
		this.textField.text = favours.ToString();
		this.textField.fontSize = (int)((float)littleFontSize + (((float)bigFontSize - (float)littleFontSize) * percentile));
		this.textField.color = Color.Lerp(littleColor, bigColor, percentile);
	}
}
