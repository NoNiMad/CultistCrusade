using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Spell : MonoBehaviour {
    public FavourManager favourManager;
    public int favourCost;
    public float cooldown;
    public Button button;

    protected bool reloading;
    protected float reloadAmount;

    protected virtual void Update()
    {
        if (reloading)
        {
            reloadAmount += Time.deltaTime;
            if (reloadAmount > GetReloadTime())
            {
                reloading = false;
                button.interactable = true;
            }
        }
    }

    public virtual bool CanCast()
    {
        return !reloading && favourManager.HowManyFavours() >= favourCost;
    }

    public float GetReloadTime()
    {
        return cooldown;
    }

    public void StartReloading()
    {
        reloading = true;
        reloadAmount = 0;
        button.interactable = false;
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

    public virtual void Cast()
    {
        StartReloading();
        favourManager.AddFavours(-favourCost);
    }

}
