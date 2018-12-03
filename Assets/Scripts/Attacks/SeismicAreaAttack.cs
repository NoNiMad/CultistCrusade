using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeismicAreaAttack : Attack {
	public int damage = 20;
    public float timeToResolve = 0.5f;
    public float size = 5;
    public GameObject prefab;

	public override void Execute() {
        GameObject tmp;
        AreaAttackResolver areaAttack;

        if (tracker.GetAllTargets().Count > 4)
        {
		StartReloading();
        if (this.GetCharAnimator())
        {
            this.GetCharAnimator().SetTrigger("SeismicAreaAttack");
        }
        tmp = Instantiate(prefab);
        tmp.transform.position = new Vector3(this.transform.position.x, 0, this.transform.position.z);
        tmp.transform.localScale = new Vector3(size, 1, size);
        areaAttack = tmp.GetComponent<AreaAttackResolver>();
        areaAttack.favourManager = this.favourManager;
        areaAttack.delay = this.timeToResolve;
        areaAttack.damages = this.damage;
        areaAttack.favoursOnKill = this.favoursOnKill;
        }
	}
}
