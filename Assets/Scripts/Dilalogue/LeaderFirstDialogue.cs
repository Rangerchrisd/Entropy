using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderFirstDialogue : Dialogue
{
    public Mission newTutorialMission;
    public List<Hex> consoleSquares;

    public override void Start()
    {
        base.Start();
        gameManager.missions.Add(newTutorialMission);
    }
}
