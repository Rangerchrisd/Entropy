using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MissionConsole : Interactable
{
    public GameObject missionSellectObject;
    public override void interact(Character character)
    {
        base.interact(character);
        missionSellectObject.SetActive(true);
    }

}
