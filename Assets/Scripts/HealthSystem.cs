using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour {
    public int maxHealth;
    [HideInInspector]
    public int health;

    void Start()
    {
        health = maxHealth;
    }

    public bool Damage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Destroy(gameObject);
            return true;
        }
        return false;
    }
}
