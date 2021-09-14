using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "Ability/MedPack")]
public class MedPack : Ability
{
    public int healing;

    public override void doAbility(Hex targetHex, Unit user)
    {
        base.doAbility(targetHex, user);
        targetHex.myUnit.healDamage(healing);
    }

}
