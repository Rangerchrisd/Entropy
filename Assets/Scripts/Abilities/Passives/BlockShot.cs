using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockShot : Passive
{
    public int numberOfBlocks;
    public override void Start()
    {
        Unit.onTakeDamage += Unit_onTakeDamage;
    }
    private void Unit_onTakeDamage(Unit arg1, float arg2, Unit arg3)
    {
        if (numberOfBlocks > 0)
        {
            if (arg1 == myCharacter)
            {
                arg1.hp += arg2;
                numberOfBlocks--;
            }
        }
    }
    public override void OnDestroy()
    {
        Unit.onTakeDamage -= Unit_onTakeDamage;

    }
}
