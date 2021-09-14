using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathMatch : Objective
{
    public override void Update()
    {
        if (playerTurn)
        {
            base.Update();
        }
        else
        {
            playerTurn = FindObjectOfType<PlayerTurn>();
        }
    }
    public override bool winCondition() {
        if (combatManager.enemies.Count == 0) {
            return true;
        }
        return false;
    }
    public override void win()
    {
        base.win();
    }
}
