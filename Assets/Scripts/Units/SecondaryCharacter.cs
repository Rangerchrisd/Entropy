using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class SecondaryCharacter : Character
{
    public static event Action<SecondaryCharacter> secondaryDie;
    public static event Action<SecondaryCharacter> spawnedSecondary;
    public override void Start()
    {
        base.Start();
        spawnedSecondary?.Invoke(this);
    }
    public int doPlayerTurn()
    {
        return 1;
    }
    public override void die()
    {
        secondaryDie?.Invoke(this);
        base.die();
    }
    public override void OnDestroy()
    {
        base.OnDestroy();
    }
}
