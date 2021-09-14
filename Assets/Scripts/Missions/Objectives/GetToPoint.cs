using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetToPoint : Objective
{
    public List<Hex> endPoints;
    public bool won;
    public int flag = -1;
    public GameObject gameObjectToSpawn;
    public override void Start()
    {
        base.Start();
        Unit.onMoveSquare += Unit_onMoveSquare;
    
    }
    public override void OnDestroy()
    {
        base.OnDestroy();
        Unit.onMoveSquare -= Unit_onMoveSquare;
    }

    private void Unit_onMoveSquare(Unit obj)
    {
        if (obj is PrimaryCharacter) {
            if (endPoints.Contains(obj.myHex)) {
                win();
            }
        }
    }

    public override void win()
    {

        if (gameObjectToSpawn)
            Instantiate(gameObjectToSpawn);
        if (flag != -1)
        {
            if(flag<playerTurn.gameManager.player.storyFlags.Length)
                playerTurn.gameManager.player.storyFlags[flag] = true;
        }
        Destroy(this);
    }
}
