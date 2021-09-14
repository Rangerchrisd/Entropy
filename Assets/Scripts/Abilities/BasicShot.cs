using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "Ability/BasicShot")]
public class BasicShot : Ability
{
    public int shotDamage;
    public override void doAbility(Hex targetHex, Unit user)
    {
        base.doAbility(targetHex, user);
        targetHex.myUnit.takeDamage(shotDamage,user);
    }
}
