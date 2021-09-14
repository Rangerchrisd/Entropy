using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueConversation : Interactable
{
    public GameObject dialoguePrefab;
    public override void interact(Character character)
    {
        base.interact(character);
        Instantiate(dialoguePrefab, Vector3.zero, Quaternion.identity);
    }
}
