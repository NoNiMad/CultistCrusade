using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaAttackResolver : MonoBehaviour {
	public Color beginingColor;
	public Color endingColor;
	public FavourManager favourManager;
	public int favoursOnKill;
	public int damages;
	public float delay;
    float elapsed;
	public EntitySide toAttack;

	void start() {
	}

	void Update () {
        elapsed += Time.deltaTime;

		this.GetComponentInChildren<Renderer>().material.color = Color.Lerp(beginingColor, endingColor, elapsed / delay);
		
        if (elapsed > delay)
        {
			Collider[] hitColliders = Physics.OverlapSphere(this.gameObject.transform.position, this.gameObject.transform.localScale.x / 2);
            foreach (Collider collider in hitColliders) {
				HealthSystem hs = collider.gameObject.GetComponent<HealthSystem>();
				if (hs != null && collider.gameObject.GetComponent<Targetable>().GetSide() == toAttack && hs.Damage(damages))
        	    {
            	    if (favourManager != null)
                	    favourManager.AddFavours(favoursOnKill);
           		}
			}
			Destroy(this.gameObject);
        }
	}
}
