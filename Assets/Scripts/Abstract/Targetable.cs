using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Targetable : MonoBehaviour {
    public abstract EntitySide GetSide();
    public virtual void OnDeath() {
        Destroy (this.gameObject);
    }
}
