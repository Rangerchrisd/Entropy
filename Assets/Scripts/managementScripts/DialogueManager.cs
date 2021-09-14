using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
public class DialogueManager : MonoBehaviour
{
    public GameObject buttonPrefab;
    public Image profilePic;
    public TMP_Text profileName;
    public TMP_Text dialogueLine;
    public Image Background;
    public Image profileNamePlate;
    public Vector3[] buttonPosistions;
    public List<GameObject> answerButtons = new List<GameObject>();
    public CombatManager combatManager;
    /// <summary>
    /// <param name="read">The dialogue.</param>
    /// <param name="face">The image for the character.</param>
    /// <param name="personName">The name of who is talking.</param>
    /// </summary>
    public void newLine(string personName, Sprite face, string read) {
        if (!face) {
            profilePic.enabled = false;
        }
        else
        {
            profilePic.enabled = true;
            profilePic.sprite = face;
        }
        if (personName.Length<=0) {
            profileName.enabled = false;
            profileNamePlate.enabled = false;
        }
        else {
            profileName.enabled = true;
            profileNamePlate.enabled = true;
            profileName.text = personName;
        }
        dialogueLine.text = read;
    }
    public void newQuestion(string[] questions) {
        int i = 0;
        foreach (string question in questions)
        {
            DialogueAnswer button =  Instantiate(buttonPrefab, 
                buttonPosistions[i], Quaternion.identity,this.transform).GetComponent<DialogueAnswer>();
            RectTransform rectTransform = button.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(rectTransform.rect.x, buttonPosistions[i].y);
            button.myText.text = question;
            button.answer = i;
            i++;
            answerButtons.Add(button.gameObject);
        }
    }
    public void newBackground(Sprite newBackground)
    {
        if (newBackground == null)
        {
            Background.enabled = false;
        }
        else {
            Background.sprite = newBackground;
        }

    }
    public void destroyAnswers() {
        foreach (GameObject i in answerButtons)
            Destroy(i);
    }

    public bool DialogueStart()
    {
        combatManager = FindObjectOfType<CombatManager>();
        if (!combatManager)
            return false;
        combatManager.inDialogue = true;
        return true;
    }
    public void OnDestroy()
    {
        if(combatManager)
            combatManager.inDialogue = false;
    }
}
