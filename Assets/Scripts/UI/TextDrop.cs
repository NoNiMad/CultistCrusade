using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextDrop : MonoBehaviour {
    public float yOffset = 20;
    public float speed = 1;

    Vector3 initialPos;
    float xOffset;
    float timeElapsed;
    int dir;

	void Start () {
        initialPos = transform.position;

        xOffset = Random.Range(0.1f, 1.0f) * yOffset;
        speed = speed * xOffset / yOffset;
        dir = Random.value < 0.5f ? 1 : -1;
        xOffset *= dir;

        timeElapsed = 0;
	}
	
    float ApplyFunction(float x)
    {
        return ((-yOffset / (xOffset * xOffset)) * x + (2 * yOffset / xOffset)) * x;
    }

	void Update () {
        timeElapsed += Time.deltaTime * speed * dir;
        transform.position = initialPos + new Vector3(timeElapsed, ApplyFunction(timeElapsed));
	}
}
