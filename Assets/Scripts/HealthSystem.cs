using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour {
    public delegate void KilledDelegate(Targetable target);
    public event KilledDelegate OnKilled;

    public int maxHealth;
    [HideInInspector]
    public int health;

    GameObject healthBarPrefab;
    Transform healthBarParent;
    Transform healthBarChild;

    void Start()
    {
        health = maxHealth;
        healthBarPrefab = Resources.Load("Prefab/HealthBar") as GameObject;
        healthBarParent = Instantiate<GameObject>(healthBarPrefab, transform).transform;
        healthBarParent.localPosition += Vector3.up * 2;
        healthBarChild = healthBarParent.GetChild(0);
    }

    void Update()
    {
        float healthPercentage = (float)health / (float)maxHealth;
        healthBarParent.LookAt(transform.position + (transform.position - Camera.main.transform.position) * 2);
        healthBarChild.transform.localScale = new Vector3(healthPercentage, 1, 1);
        healthBarChild.transform.localPosition = new Vector3((healthPercentage - 1) / 2, 0, -0.0001f);
    }

    public bool Damage(int amount)
    {
        health -= amount;
        if (health > maxHealth)
        {
            health = maxHealth;
        }

        if (health <= 0)
        {
            OnKilled(GetComponent<Targetable>());
            Destroy(gameObject);
            return true;
        }

        healthBarParent.gameObject.SetActive(health <= maxHealth);
        return false;
    }
}
