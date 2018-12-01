using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : Targetable {
    Attack mainAttack;

    void Start()
    {
        mainAttack = GetComponent<Attack>();
    }

    void Update()
    {
        if (mainAttack.CanExecute())
        {
            mainAttack.Execute();
        }
    }

    public override EntitySide GetSide()
    {
        return EntitySide.FRIENDLY;
    }
}
