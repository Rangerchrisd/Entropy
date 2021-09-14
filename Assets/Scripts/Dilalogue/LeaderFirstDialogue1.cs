using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderFirstDialogue1 : MonoBehaviour
{
    public bool acceptingInput;
    //public string[] dialogueLines = new string[0];
    public List<string> dialogueLines = new List<string>();
    public int lineNumber = 0;
    public int dialogueLinesCount;
    public DialogueManager dialogueManagerPrefab;
    public Dialogue nextLevelDialogue;
    public DialogueManager dialogueManager;

    public string dialogueLine;
    public string characterName;
    public string dialogueDescription;
    public Sprite characterImage;
    public Sprite[] MyImages;

    public PlayerTurn partyInformation;
    public Mission newTutorialMission;

    public Sprite getImage(int index) {
        if (MyImages.Length < index)
        {
            return null;
        }
        return MyImages[index];
    }
    public Sprite getImage(string faceIndex)
    {
        int index = int.Parse(faceIndex);
        return getImage(index);
    }
    public virtual void Start()
    {
        DialogueAnswer.onAnswer += answerQuestion;
        findDialogueManager();
        partyInformation =  FindObjectOfType<PlayerTurn>();
        partyInformation.acceptInstructions = false;
        dialogueLinesCount = dialogueLines.Count;
        parseLine();
        
    }
    public void findDialogueManager() {

        if (!dialogueManager)
        {
            dialogueManager = Instantiate(dialogueManagerPrefab);
        }
    }
    public void OnDestroy()
    {
        DialogueAnswer.onAnswer -= answerQuestion;
    }
    public virtual void Update()
    {

        if (acceptingInput)
        {
            CheckInput();
        }
    }
    public void answerQuestion(int answer) {

    }
    public void CheckInput() {
        if (Input.GetKeyDown(KeyCode.Space)||Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) {
            if (lineNumber+1 < dialogueLinesCount)
            {
                lineNumber++;
                parseLine();
            }
            else {
                partyInformation.acceptInstructions = true;
                Destroy(dialogueManager.gameObject);
                Destroy(this);
            
            }
        }
    }
    public void parseLine() {

        processLine();
        dialogueManager.newLine(characterName,characterImage,dialogueLine);

    }
    public void resetLine()
    {
        characterImage = null;
        characterName = "";
        dialogueLine = "";
    }
    public void processLine()
    {
        resetLine();
        if (lineNumber >= dialogueLinesCount)
        {
            Debug.Log("ended weirdly");
            //caused by background/check at end of a conversation
            Destroy(dialogueManager.gameObject);
            Destroy(this);
            return;
        }
        string read = dialogueLines[lineNumber];
        if (read.StartsWith("q:"))
        {
            acceptingInput = false;
            read = read.Substring(2);
            string[] answers = read.Split(':');
            dialogueManager.newQuestion(answers);
            return;
        }
        else if (read.StartsWith("b:"))
        {
            read = read.Substring(2);
            int backgroundIndex = int.Parse(read);
            if (backgroundIndex == -1)
            {
                dialogueManager.Background.sprite = null;

            }
            else
            {
                if (backgroundIndex < MyImages.Length&&backgroundIndex>=0)
                    dialogueManager.newBackground(MyImages[backgroundIndex]);
            }
            lineNumber++;
            parseLine();
            return;
        }
        else{
            if (partyInformation) {
                if (read.StartsWith("?:"))
                {
                    read = read.Substring(2);
                    string[] characterLookingFor = read.Split(':');
                    if (partyInformation.hasCharacter(characterLookingFor[0]))
                    {
                        read = read.Substring(read.IndexOf(':'));
                    }
                    else
                    {
                        lineNumber++;
                        parseLine();
                        return;
                    }
                } else if (read.StartsWith("p:"))
                {
                    string[] flagLookingFor = read.Split(':');
                    if (int.TryParse(flagLookingFor[1], out int flag))
                    {
                        if (partyInformation.gameManager.player.storyFlags[flag])
                        {
                            read = read.Substring(read.IndexOf(':'));
                            read = read.Substring(read.IndexOf(':'));
                        }
                        else
                        {
                            lineNumber++;
                            parseLine();
                            return;
                        }
                    }
                    else {
                        Debug.Log("put a number in after the p");
                    }
                }
            }
        }
        string[] words = read.Split(':');
        if (words.Length == 0) {
            return;
        }
        else if (words.Length == 1) {
            dialogueLine = words[0];
            
        } else if (words.Length == 2)
        {
            characterImage = getImage(words[0]);
            dialogueLine = words[1];
        } else if (words.Length == 3)
        {
            characterName = words[0];
            characterImage = getImage(words[1]);
            dialogueLine = words[2];
        }
        else {
            if (read.StartsWith("?:")){
                Debug.Log("you forgot to press the needsParty button or it couldn't find it");
            }
            else {
                Debug.Log("you fool." +
                    " you absolute buffoon. you think you can challenge me in my own realm?" +
                    " you think you can rebel against my authority? ");
            }
        }
    }
}
