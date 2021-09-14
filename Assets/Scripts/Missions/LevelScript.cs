using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScript : MonoBehaviour
{
    public GameManager gameManager;
    public Dialogue dialogue;
    // Start is called before the first frame update
    public virtual void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }
    public void loadDialogue(GameObject dialogueToLoad) {
        dialogue = Instantiate(dialogueToLoad).GetComponent<Dialogue>();
        dialogue.findDialogueManager();
        if (dialogue.dialogueManager.DialogueStart())
        {
            Debug.Log("I want this to win race condition");
        }

    }
    public bool checkDoor(DoorTile doorTileDoorTitle) {
        if (doorTileDoorTitle.opened)
            return true;
        return false;
    }

    public void finishLevel() {
        gameManager.missions.Remove(gameManager.currentMission);
        gameManager.goBackHome();
    }
    public void highlightHexes(List<Hex> hexes) {

        foreach (Hex i in hexes)
        {
            i.highlightHex();
        }
    }
    public void unhighlightHexes(List<Hex> hexes)
    {

        foreach (Hex i in hexes)
        {
            i.unhighlightHex();
        }
    }
}
