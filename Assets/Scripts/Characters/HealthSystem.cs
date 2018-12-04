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

    AudioSource audioEmitter;
    Targetable character;

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
        
        audioEmitter =  this.GetComponent<AudioSource>();
        character = this.GetComponent<Targetable>();
    }

    void Update()
    {
        float healthPercentage = (float)health / (float)maxHealth;
        healthBarParent.transform.position = Camera.main.WorldToScreenPoint(transform.position) + Vector3.up * 30;
        healthBarChild.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, healthPercentage * 50);
    }

    public bool Damage(int amount)
    {
        int i = character.hurtClips.Length;

        i = Random.Range(0, i - 1);
        health -= amount;
        if (amount > 0 && character.hurtClips.Length > 0 && health > 0)
            audioEmitter.PlayOneShot(character.hurtClips[i]);

        if (health > maxHealth)
        {
            health = maxHealth;
        } else
        {
            SpawnTextParticle(amount);
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

        healthBarParent.gameObject.SetActive(health < maxHealth);
        return false;
    }

    Color minGreen = new Color(0, 1, 0, 0.8f);
    Color maxGreen = new Color(0, 1, 0);
    Color minDamage = new Color(1, 1, 0, 0.8f);
    Color maxDamage = new Color(1, 0, 0);

    void SpawnTextParticle(int amount)
    {
        GameObject damageText = Instantiate(damageTextPrefab, inGameUIContainer);
        damageText.transform.position = Camera.main.WorldToScreenPoint(transform.position) + Random.insideUnitSphere * 20;

        TMPro.TextMeshProUGUI textMesh = damageText.GetComponent<TMPro.TextMeshProUGUI>();
        textMesh.text = Mathf.Abs(amount).ToString();
        float percentage = Mathf.Clamp((float)amount / 200f, 0f, 1f);
        if (amount > 0)
        {
            textMesh.color = Color.Lerp(minDamage, maxDamage, percentage);
        }
        else if (amount < 0)
        {
            textMesh.color = Color.Lerp(minGreen, maxGreen, percentage);
        }

    }
}
