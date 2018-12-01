using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemy : MonoBehaviour, Targetable {    
    Attack mainAttack;

	void Start ()
    {
        mainAttack = GetComponent<Attack>();
	}
	
	void Update ()
    {
        if (mainAttack.CanExecute())
        {
            mainAttack.Execute();
        }
	}

    public EntitySide GetSide()
    {
        return EntitySide.ENNEMY;
    }
}
