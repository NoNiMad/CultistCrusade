using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDebugger : MonoBehaviour {
    TargetTracker tracker;

	void Start () {
        tracker = GetComponent<TargetTracker>();
	}

    void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            foreach (Targetable t in tracker.targets)
            {
                Gizmos.DrawLine(transform.position, (t as MonoBehaviour).transform.position);
            }
        }
    }
}
