using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgSoundSingleton : MonoBehaviour
{

	private static BgSoundSingleton instance = null;

	public static BgSoundSingleton Instance => instance;

	private void Awake()
	{
		if (instance != null && instance != this)
		{
			Destroy(this.gameObject);
			return;
		}
		else
		{
			instance = this;
		}
		DontDestroyOnLoad(this.gameObject);
	}
}
