using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScientistDialogue : Dialogue
{
    
    // Start is called before the first frame update
    public override void Start()
    {


        gameManager = FindObjectOfType<GameManager>();

        DialogueAnswer.onAnswer += answerQuestion;
        findDialogueManager();
        dialogueLinesCount = dialogueLines.Count;

        partyInformation = FindObjectOfType<PlayerTurn>();

        partyInformation.acceptInstructions = false;
        //this breaks if the two are not connected
        if (partyInformation.gameManager.player)
        {
            if (partyInformation.gameManager.player.storyFlags[2])
            {
                partyInformation.gameManager.player.storyFlags[3] = true;
                dialogueLines.Insert(4, "oh you already have");
            }
        }
        dialogueLinesCount = dialogueLines.Count;
        parseLine();
    }
}
