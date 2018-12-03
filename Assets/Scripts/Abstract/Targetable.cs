using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Targetable : MonoBehaviour {
    public AudioClip[] hurtClips;
    public AudioClip[] deathClips;
    
    public abstract EntitySide GetSide();
    public virtual void OnDeath() {
        Destroy (this.gameObject);
    }
}
