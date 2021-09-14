using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective : MonoBehaviour
{
    public CombatManager combatManager;
    public PlayerTurn playerTurn;
    public virtual void Start() {
        combatManager = FindObjectOfType<CombatManager>();
        playerTurn = combatManager.gameManager.playerTurn;
    }
    public virtual void OnDestroy() { 
    
    
    }
    // Update is called once per frame
    public virtual void Update()
    {

        if (winCondition()) {
            win();
        }
    }
    public virtual bool winCondition()
    {

        return false;
    }
    public virtual void win() {

        Debug.Log("win");
    }
}
