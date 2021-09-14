using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Interactable : MonoBehaviour
{
    public bool nextSpaceInteractable;
    public static event Action<Character,Interactable> OnInteract;
    public virtual void interact(Character character) {
        OnInteract?.Invoke(character, this);

    }
}
