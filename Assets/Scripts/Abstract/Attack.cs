using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : MonoBehaviour {
    protected TargetTracker tracker;

    protected bool reloading;
    protected float reloadAmount;

    void Start()
    {
        tracker = GetComponent<TargetTracker>();
        reloading = false;
        reloadAmount = 0;
    }

    void Update()
    {
        if (reloading)
        {
            reloadAmount += Time.deltaTime;
            if (reloadAmount > GetReloadTime())
            {
                reloading = false;
            }
        }
    }

    public bool CanExecute()
    {
        return !reloading;
    }

    // Do the attack
    public abstract void Execute();

    // Cooldown in seconds
    public abstract float GetReloadTime();

    public void StartReloading()
    {
        reloading = true;
        reloadAmount = 0;
    }

    // Progress from 0.0 to 1.0 for the reload
    public float GetReloadProgess()
    {
        if (reloading)
        {
            return reloadAmount / GetReloadTime();
        }
        return 1;
    }
}
