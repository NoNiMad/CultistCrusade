using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTracker : MonoBehaviour {
    public int maxTracked = -1;

    [HideInInspector]
    public List<Targetable> targets;

    void Start()
    {
        targets = new List<Targetable>();
    }

    public void AddTarget(Targetable target)
    {
        if (maxTracked == -1 || targets.Count < maxTracked)
        {
            targets.Add(target);
        }
    }

    public void RemoveTarget(Targetable target)
    {
        targets.Remove(target);
    }
    
    public Targetable GetRandomTarget()
    {
        if (targets.Count == 0) return null;
        return targets[Random.Range(0, targets.Count - 1)];
    }

    public List<Targetable> GetRandomTargets(int n)
    {
        List<Targetable> result = new List<Targetable>();
        if (targets.Count == 0) return result;

        while (result.Count < n)
        {
            Targetable t = GetRandomTarget();
            if (!targets.Contains(t))
            {
                targets.Add(t);
            }
        }
        return result;
    }
}
