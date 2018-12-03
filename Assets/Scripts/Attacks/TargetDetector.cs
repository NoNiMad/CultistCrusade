using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetector : MonoBehaviour {
    public EntitySide targetSide;

    private void OnTriggerEnter(Collider other)
    {
        Targetable target = other.GetComponent<Targetable>();
        if (target != null && targetSide == target.GetSide())
        {
            GetComponentInParent<TargetTracker>().AddTarget(target);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Targetable target = other.GetComponent<Targetable>();
        if (target != null && targetSide == target.GetSide())
        {
            GetComponentInParent<TargetTracker>().RemoveTarget(target);
        }
    }
}
