using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.EventSystems;

public class AbilityButton : MonoBehaviour, IPointerDownHandler
{
    public Ability ability;
    public TMP_Text myText;
    public PlayerTurn playerTurn;
    public void OnPointerDown(PointerEventData eventData)
    {
        ability.targetAbility(playerTurn);
    }
    public void loadAbility(Ability abilityToLoad) {
        ability = abilityToLoad;
        myText.text = ability.abilityName;
    }
}
