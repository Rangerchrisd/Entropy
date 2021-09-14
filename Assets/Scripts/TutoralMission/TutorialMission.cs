using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

/// <summary>
/// attack, end turn, move, options
/// starts off with person moving arround going to the door
/// An event takes place and it effects this
/// maybe npc dies
/// 
/// </summary>



public class TutorialMission : LevelScript
{
    public bool doneLevel, waitLoad, midLevel;
    public List<Hex> consoleSquares;
    public DoorTile myDoor;
    public Interactable consoleInteract;
    public GameObject dialogueOnFinishFight;
    public GameObject dialogueOnGetToEndPoints;


    public int missionStage;
    //missionstage 0: they arrive in a hallway with a door have character walk ahead and open it.
    //missionstage 1: the door ahead is opened and they have to use a console
    //missionstage 2: main character has to make science check
    //missionstage 3: ltn go gets hit for a lot of damage, but survives. Their entropy value goes up.
    //missionstage 4: "at least its safe now" entropy goes up a lot, allien appears kills the ltn
    //missionstage 5: player kills allien and then leaves
    //missionstage 6: no exp reward, but end screen says tech unlock.

    public override void Start()
    {
        base.Start();
        CombatManager.OnEndCombat += CombatManager_OnEndCombat;
        PrimaryCharacter.onMoveSquare += PrimaryCharacter_onMoveSquare;
        DoorTile.doorOpened += DoorTile_doorOpened;
        Interactable.OnInteract += Interactable_OnInteract;
        missionStage = 0;
    }

    private void Interactable_OnInteract(Character characterInteracting, Interactable interactableInteracting)
    {
        if (characterInteracting.unitName == "MyCharacter")
        {
            if (interactableInteracting == consoleInteract)
            {
                gameManager.effectTextManager.doText("this isn't in yet", 1.5f, consoleInteract.transform.position);
            }
        }
        else {
            gameManager.effectTextManager.doText("have main character interact with it instead",1.5f, consoleInteract.transform.position);
        }
    }

    private void PrimaryCharacter_onMoveSquare(Unit obj)
    {
        if (missionStage == 1)
        {
            //replace with interact on the thing
            if (consoleSquares.Contains(obj.myHex))
            {
                missionStage++;
            }
        }
    }

    private void CombatManager_OnEndCombat()
    {
        //should be 3 in the end
        if (missionStage==0)
        {
            highlightHexes(consoleSquares);
        }
    }

    private void DoorTile_doorOpened(DoorTile arg1, Hex arg2)
    {
        if (missionStage ==0) { 
        
        }
    }

    public void OnDestroy()
    {
        CombatManager.OnEndCombat -= CombatManager_OnEndCombat;
        PrimaryCharacter.onMoveSquare -= PrimaryCharacter_onMoveSquare;
        DoorTile.doorOpened += DoorTile_doorOpened;
        Interactable.OnInteract -= Interactable_OnInteract;
    }
}
