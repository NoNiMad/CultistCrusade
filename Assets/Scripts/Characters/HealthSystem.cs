using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Targetable))]
public class HealthSystem : MonoBehaviour {
    public delegate void DamageDelegate(Targetable target);
    public event DamageDelegate OnDamage;

    public delegate void KilledDelegate(Targetable target);
    public event KilledDelegate OnKilled;

    public int maxHealth;
    [HideInInspector]
    public int health;

    RectTransform inGameUIContainer;

    GameObject healthBarPrefab;
    GameObject damageTextPrefab;

    RectTransform healthBarParent;
    RectTransform healthBarChild;

    void Start()
    {
        health = maxHealth;

        inGameUIContainer = GameObject.Find("/Canvas/InGame").GetComponent<RectTransform>();

        healthBarPrefab = Resources.Load("Prefab/HealthBar") as GameObject;
        healthBarParent = Instantiate(healthBarPrefab, inGameUIContainer).GetComponent<RectTransform>();
        healthBarParent.transform.position = Camera.main.WorldToScreenPoint(transform.position) + Vector3.up * 30;
        healthBarChild = healthBarParent.GetChild(0).GetComponent<RectTransform>();

        damageTextPrefab = Resources.Load("Prefab/DamageText") as GameObject;
    }

    void Update()
    {
        float healthPercentage = (float)health / (float)maxHealth;
        healthBarParent.transform.position = Camera.main.WorldToScreenPoint(transform.position) + Vector3.up * 30;
        healthBarChild.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, healthPercentage * 50);
    }

    public bool Damage(int amount)
    {
        health -= amount;
        if (health > maxHealth)
        {
            health = maxHealth;
        }

        if (OnDamage != null)
            OnDamage(GetComponent<Targetable>());

        if (health <= 0)
        {
            if (OnKilled != null)
                OnKilled(GetComponent<Targetable>());
            Destroy(healthBarParent.gameObject);
            gameObject.GetComponent<Targetable>().OnDeath();
            return true;
        }

        healthBarParent.gameObject.SetActive(health <= maxHealth);
        GameObject text = Instantiate(damageTextPrefab, inGameUIContainer);
        text.transform.position = Camera.main.WorldToScreenPoint(transform.position) + Random.insideUnitSphere * 20;
        text.GetComponent<TMPro.TextMeshProUGUI>().text = amount.ToString();
        return false;
    }
}
