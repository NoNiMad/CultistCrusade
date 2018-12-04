﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour {

	public void SwitchToGame(string SceneName)
	{
		Debug.Log(SceneName);
		SceneManager.LoadScene(SceneName);
	}
}
