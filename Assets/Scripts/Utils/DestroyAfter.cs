using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour {
    public float delay;
    float elapsed;

	void Update () {
        elapsed += Time.deltaTime;
        if (elapsed > delay)
        {
            Destroy(gameObject);
        }
	}
}
