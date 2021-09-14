using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

public enum turnType
{
    None,
    PlayerTurn,
    secondaryUnitTurn,
    Enemy,
    NeutralTurn,
    endOfRound
}
public class CombatManager : MonoBehaviour
{

    public List<Hex> allHexes = new List<Hex>();
    public GameManager gameManager;
    public List<Enemy> enemies;
    public List<PrimaryCharacter> primaryCharacters;
    public List<SecondaryCharacter> secondaryCharacters;
    public turnType activeTurn;
    public bool isPlayerTurn = true, inCombat = false,hasLoaded = false, inDialogue = false, combatWaiting = false;
    public static event Action OnEndRound;
    public static event Action OnStartCombat;
    public static event Action OnEndCombat;


    public void TurnEnded(turnType turnEnded) {
        if (turnType.PlayerTurn == turnEnded)
        {
            activeTurn = turnType.Enemy;
            gameManager.enemyPlayerTurn.newRound();
        }
        else if (turnType.Enemy == turnEnded)
        {
            activeTurn = turnType.PlayerTurn;
            OnEndRound?.Invoke();
            gameManager.playerTurn.newRound();

        }
    }

    public void Start()
    {
        newMap();
        Hex.spawnHex += addHexes;
        Hex.removeHex += removeHex;
        Enemy.spawnedEnemy += spawnEnemy;
        Enemy.enemyDie += enemyDeath;
        Enemy.enemyDespawn += enemyDeath;
        PrimaryCharacter.spawnPrimary += spawnPrimaryAlly;
        SecondaryCharacter.spawnedSecondary += spawnSecondaryAlly;
        PrimaryCharacter.primaryDespawn += PrimaryAllyDespawn;
        PrimaryCharacter.primaryDie += PrimaryAllyDeath;
        SecondaryCharacter.secondaryDie += SecondaryAllyDeath;
        
    }

    public void OnDestroy()
    {
        Hex.spawnHex -= addHexes;
        Hex.removeHex -= removeHex;
        Enemy.spawnedEnemy -= spawnEnemy;
        Enemy.enemyDie -= enemyDeath;
        Enemy.enemyDespawn -= enemyDeath;
        PrimaryCharacter.spawnPrimary -= spawnPrimaryAlly;
        SecondaryCharacter.spawnedSecondary -= spawnSecondaryAlly;
        PrimaryCharacter.primaryDie -= PrimaryAllyDeath;
        PrimaryCharacter.primaryDespawn -= PrimaryAllyDespawn;
        SecondaryCharacter.secondaryDie -= SecondaryAllyDeath;
    }
    public void Update()
    {
        if (!hasLoaded) {
            checkIfLoaded();
        }
        if (combatWaiting) {
            combatWaiting = false;
            startCombat();
        }
    }
    //called when object is destroyed in scene
    public void OnApplicationQuit()
    {
        OnDestroy();
    }
    public void newMap()
    {
        allHexes.Clear();

        Hex[] hexAlreadyHere = FindObjectsOfType<Hex>();
        foreach (Hex i in hexAlreadyHere)
            addHexes(i);
        /*
        Enemy[] alreadyHereEnemy = FindObjectsOfType<Enemy>();
        foreach (Enemy i in alreadyHereEnemy)
            spawnEnemy(i);
        SecondaryCharacter[] tempsecondaryCharacters = FindObjectsOfType<SecondaryCharacter>();
        foreach (SecondaryCharacter i in tempsecondaryCharacters)
            spawnSecondaryAlly(i);
    */
    }

    public bool checkIfLoaded()
    {
        foreach (PrimaryCharacter characterMember in gameManager.playerTurn.myParty)
        {
            if (!characterMember.hasLoaded)
            {
                return false;
            }
        }
        if (!gameManager)
        {
            return false;
        }
        if (!gameManager.player)
        {
            return false;
        }
        //primaryCharacterLoadSheets();
        hasLoaded = true;
        gameManager.playerTurn.hasLoaded = true;
        gameManager.playerTurn.changeCharacter(0);
        return true;
    }
    public void startCombat()
    {
        if (inDialogue) {
            combatWaiting = true;
            return;
           }

        OnStartCombat?.Invoke();
        if (hasLoaded)
            gameManager.playerTurn.interruptMoving();
        inCombat = true;
        
        foreach (Character i in gameManager.playerTurn.myParty)
            i.readyCombat();
        gameManager.playerTurn.NonCombatHUD.SetActive(false);
        gameManager.playerTurn.combatHUDGameObject.SetActive(true);
        if (gameManager.combatManager.enemies.Count == 0)
            gameManager.playerTurn.BackToNonCombat.SetActive(true);
    }
    public void endCombat()
    {


        OnEndCombat?.Invoke();
        gameManager.combatManager.inCombat = false;
        gameManager.playerTurn.targettingAbility = null;
        gameManager.playerTurn.NonCombatHUD.SetActive(true);
        gameManager.playerTurn.combatHUDGameObject.SetActive(false);
        gameManager.playerTurn.BackToNonCombat.SetActive(false);
    }
    #region events

    public void addHexes(Hex hexToAdd)
    {
        allHexes.Add(hexToAdd);
    }
    public void removeHex(Hex hexToRemove)
    {
        allHexes.Remove(hexToRemove);
    }
    public void spawnEnemy(Enemy newEnemy)
    {
        if (!inCombat)
        {
            
            startCombat();
        }
        if (gameManager.playerTurn.BackToNonCombat.activeSelf)
            gameManager.playerTurn.BackToNonCombat.SetActive(false);
        enemies.Add(newEnemy);
    }
    public void spawnPrimaryAlly(PrimaryCharacter character)
    {
        primaryCharacters.Add(character);
    }
    public void spawnSecondaryAlly(SecondaryCharacter character)
    {
        secondaryCharacters.Add(character);
    }
    public void PrimaryAllyDespawn(PrimaryCharacter character)
    {
        //remove from targets
        primaryCharacters.Remove(character);
        //remove from party
        gameManager.playerTurn.myParty.Remove(character);
        //remove from character pool on second thought don't do this
        //gameManager.characterPool.Remove(character.myCharacter);
        
        //make sure you aren't on the wrong index
        gameManager.playerTurn.characterActiveIndex = 0;
        //if all main characters died
        if (gameManager.playerTurn.myParty.Count == 0)
        {
            //lose game
            //SceneManager.LoadScene(0);
            //WOW oops, uhh this will get called every time a scene is unloaded
        }
    }
    public void PrimaryAllyDeath(PrimaryCharacter character)
    {
        //remove from targets
        primaryCharacters.Remove(character);
        //remove from party
        gameManager.playerTurn.myParty.Remove(character);
        //remove from character pool
        gameManager.characterPool.Remove(character.myCharacter);
        //make sure you aren't on the wrong index
        gameManager.playerTurn.characterActiveIndex = 0;
        //if all main characters died
        if (gameManager.playerTurn.myParty.Count == 0 ) {
            //lose game
            Debug.Log("why");
            SceneManager.LoadScene(0);
            //WOW oops, uhh this will get called every time a scene is unloaded
        }
    }
    public void SecondaryAllyDeath(SecondaryCharacter character)
    {
        secondaryCharacters.Remove(character);
    }
    public void enemyDeath(Enemy deadEnemy)
    {
        enemies.Remove(deadEnemy);
        if (enemies.Count == 0)
        {
            endCombat();
        }
    }
    #endregion
    #region returnZones
    public List<Hex> findEnemies()
    {
        List<Hex> returner = new List<Hex>();
        foreach (Enemy i in enemies)
        {
            returner.Add(i.myHex);
        }
        return returner;
    }
    public List<Hex> findEnemies(int range)
    {
        List<Hex> returner = new List<Hex>();
        foreach (Enemy i in enemies)
        {
            if (i.myHex.distanceTo <= range)
                returner.Add(i.myHex);
        }
        return returner;
    }
    public List<Hex> findInRangeHexMove(int rangeOfMove)
    {
        List<Hex> returner = new List<Hex>();
        foreach (Hex i in allHexes)
        {
            if (i.distanceTo <= rangeOfMove && !i.myUnit)
                returner.Add(i);
        }
        return returner;
    }
    public List<Hex> findAlliesInRange(int range)
    {
        List<Hex> returner = new List<Hex>();
        foreach (PrimaryCharacter i in primaryCharacters)
        {
            if (i.myHex.distanceTo <= range)
                returner.Add(i.myHex);
        }
        foreach (SecondaryCharacter i in secondaryCharacters)
        {
            if (i.myHex.distanceTo <= range)
                returner.Add(i.myHex);
        }
        return returner;
    }

    public Hex findClosestAllyInRange(int range)
    {
        Hex returner = null;
        foreach (PrimaryCharacter i in primaryCharacters)
        {
            if (i.myHex.distanceTo <= range)
            {
                if (returner)
                {
                    if (returner.distanceTo < i.myHex.distanceTo)
                        returner = i.myHex;
                }
                else {
                    returner = i.myHex;
                }
            }
        }
        foreach (SecondaryCharacter i in secondaryCharacters)
        {
            if (i.myHex.distanceTo <= range)
            {
                if (returner)
                {
                    if (returner.distanceTo < i.myHex.distanceTo)
                        returner = i.myHex;
                }
                else
                {
                    returner = i.myHex;
                }
            }
        }
        return returner;
    }
    public Hex findClosestAlly()
    {
        Hex returner = null;
        foreach (PrimaryCharacter i in primaryCharacters)
        {
            if (returner)
            {
                //swithing > and < make the enemy always target the farthest might be an interesting enemy
                if (returner.distanceTo > i.myHex.distanceTo)
                    returner = i.myHex;
            }
            else
            {
                returner = i.myHex;
            }
        }
        foreach (SecondaryCharacter i in secondaryCharacters)
        {
            if (returner)
            {
                if (returner.distanceTo > i.myHex.distanceTo)
                    returner = i.myHex;
            }
            else
            {
                returner = i.myHex;
            }
        }
        return returner;
    }
    #endregion
}
