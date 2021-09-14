using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class popupAfterDialogue : MonoBehaviour
{
    public Dialogue waitingForDialogueToEnd;
    public GameObject popup;

    // Update is called once per frame
    void Update()
    {
        if (waitingForDialogueToEnd)
            return;
        popup.SetActive(true);
    }
    public void closePopup() {
        popup.SetActive(false);
    }
    public void destroySelf() {
        Destroy(this.gameObject);
    }
}
