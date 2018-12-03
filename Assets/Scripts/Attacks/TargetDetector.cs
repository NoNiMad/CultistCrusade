using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetector : MonoBehaviour {
    public EntitySide targetSide;
    public float range;

    TargetTracker tracker;

    private void Start()
    {
        tracker = GetComponent<TargetTracker>();
    }

    private void Update()
    {
        tracker.targets.Clear();
        Collider[] close = Physics.OverlapSphere(transform.position, range, LayerMask.GetMask(targetSide == EntitySide.FRIENDLY ? "Minion" : "Enemy"));
        foreach (Collider col in close)
        {
            Targetable t = col.GetComponent<Targetable>();
            if (t != null && t.GetSide() == targetSide)
                tracker.targets.Add(t);
        }
    }
}
