using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystem : MonoBehaviour
{
    public bool debugMode;
    public bool allSets;
    GameManager gameManager;
    public bool ChangeListener, QuietMode;
    public int entropyDieThreshold, entropyDieCost;

    public int combatsWon;
    #region subscribe and find
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        subscribe();
    }

    public void Update()
    {
        if (ChangeListener) {
            if (QuietMode)
            {
                unsubscribe();
                QuietMode = false;
            }
            else
            {
                subscribe();
                QuietMode = true;

            }
            ChangeListener = false;
        }
    }
    public void subscribe() {
        Unit.onUnitDespawn += Unit_onUnitDespawn;
        Enemy.enemyDie += Enemy_enemyDie;
        PrimaryCharacter.primaryDie += PrimaryCharacter_primaryDie;
        PrimaryCharacter.onTakeDamage += PrimaryCharacter_onTakeDamage;
        PrimaryCharacter.onMoveSquare += PrimaryCharacter_onMoveSquare;
        PrimaryCharacter.OnSkillCheck += PrimaryCharacter_OnSkillCheck;

        CombatManager.OnStartCombat += CombatManager_OnStartCombat;
        CombatManager.OnEndCombat += CombatManager_OnEndCombat;
        CombatManager.OnEndRound += CombatManager_OnEndRound;
    }

    private void CombatManager_OnEndRound()
    {
        //throw new System.NotImplementedException();
    }

    private void CombatManager_OnEndCombat()
    {
        //Instantiate()
        //gameManager.playerTurn.activeCharacter
    }

    private void CombatManager_OnStartCombat()
    {
        //throw new System.NotImplementedException();
    }

    private void PrimaryCharacter_onTakeDamage(Unit dealtTo, float dealt, Unit Dealer)
    {
        if (dealtTo.hp <= dealt) {
            if (dealtTo is PrimaryCharacter)
            {
                if (((PrimaryCharacter)dealtTo).Entropy <= entropyDieThreshold)
                {
                    dealtTo.hp = dealt + 1;
                    ((PrimaryCharacter)dealtTo).Entropy += entropyDieCost;
                }
            }
        }
    }

    public void unsubscribe()
    {
        Unit.onUnitDespawn -= Unit_onUnitDespawn;
        Enemy.enemyDie -= Enemy_enemyDie;
        PrimaryCharacter.primaryDie -= PrimaryCharacter_primaryDie;
        PrimaryCharacter.onTakeDamage -= PrimaryCharacter_onTakeDamage;
        PrimaryCharacter.onMoveSquare -= PrimaryCharacter_onMoveSquare;
        PrimaryCharacter.OnSkillCheck -= PrimaryCharacter_OnSkillCheck;

        CombatManager.OnStartCombat -= CombatManager_OnStartCombat;
        CombatManager.OnEndCombat -= CombatManager_OnEndCombat;
        CombatManager.OnEndRound -= CombatManager_OnEndRound;
    }

    private void PrimaryCharacter_OnSkillCheck(Unit arg1, abilityStat arg2, int arg3)
    {
        if (arg2 == abilityStat.intelligence) {
            Debug.Log("Speak English");
            ((PrimaryCharacter)arg1).entropy -= (1 / 4 * arg3);
        } 
    }

    private void OnDestroy() { 
        unsubscribe();
    }

    #endregion
    #region event handlers

    private void Enemy_enemyDie(Enemy obj)
    {
        if(debugMode)
            Debug.Log("I think that is all of them");
    }

    private void PrimaryCharacter_onMoveSquare(Unit obj)
    {
        if (debugMode)
        {
            if (Random.Range(0, 10) < 1)
            {
                Debug.Log("Trip");
            }
        }
    }

    private void PrimaryCharacter_primaryDie(PrimaryCharacter obj)
    {

        if (gameManager.combatManager.primaryCharacters.Count!=0)
        {
            if (debugMode)
                Debug.Log("who could have foreseen this? " + obj.unitName + " died!");
        }
    }

    private void Unit_onUnitDespawn(Unit obj)
    {

    }
    public void handleMove(Unit unitThatMoved) {
        //Hex newHex;

    }
    #endregion
}
