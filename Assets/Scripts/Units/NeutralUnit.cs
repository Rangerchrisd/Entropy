using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class NeutralUnit : Unit, IPointerClickHandler
{
    public static event Action<Unit> OnClick;

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke(this);
    }
    public void doPlayerTurn()
    {


    }
}
