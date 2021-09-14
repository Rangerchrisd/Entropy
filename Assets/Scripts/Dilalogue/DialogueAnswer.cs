using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.EventSystems;
public class DialogueAnswer : MonoBehaviour, IPointerDownHandler
{
    public int answer;
    public TMP_Text myText;
    public static event Action<int> onAnswer;

    public void OnPointerDown(PointerEventData eventData)
    {
        onAnswer?.Invoke(answer);
    }
}
