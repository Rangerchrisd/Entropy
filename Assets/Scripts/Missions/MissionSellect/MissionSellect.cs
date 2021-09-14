using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
using TMPro;
using UnityEngine.EventSystems;

public class MissionSellect : MonoBehaviour
{
    public MissionSellectMissionSlot missionSlot;
    public List<MissionSellectCharacterSlot> partySlots;
    public List<MissionSellectCharacterSlot> inactiveSlots;
    public List<MissionSellectMissionSlot> missionSlots;
    public List<CharacterSheet> characterSheets = new List<CharacterSheet>();
    public GameManager gameManager;

    public float waitTime;
    public float currentTime;
    public bool loadingLevel;

    public static event Action onStartMission;
    public void Awake()
    {
        // MissionSellectCharacterSlot.sellectCharacter += handleCharacterSellectSlot;
        //  MissionSellectMissionSlot.sellectMission += handleMissionSellectSlot;
        gameManager = FindObjectOfType<GameManager>();
    }
    public void OnEnable()
    {

        loadUI();
    }
    public void OnDestroy()
    {
        //MissionSellectCharacterSlot.sellectCharacter -= handleCharacterSellectSlot;
        //MissionSellectMissionSlot.sellectMission -= handleMissionSellectSlot;
    }
    public void loadUI() {
        int i = 0;
        foreach (CharacterSheet characterSheet in gameManager.characterPool) {
            if (characterSheet.missionAble)
            {
                inactiveSlots[i].loadSlot(characterSheet);
                i++;
            }
        }
        for (; i < inactiveSlots.Count; i++)
        {
            inactiveSlots[i].loadSlot(null);
        }
        i = 0;

        foreach (Mission mission in gameManager.missions) {
            missionSlots[i].loadSlot(mission);
            i++;
        }
        for (; i < missionSlots.Count; i++)
        {
            missionSlots[i].loadSlot(null);
        }
    }
    public void loadLevel() {
        if (missionSlot.mission) {

            bool bodyFound;
            for (int i = 0; i < missionSlot.mission.charactersRequired.Count;i ++) {
                bodyFound = false;
                for (int ii = 0; ii < partySlots.Count; ii++)
                {
                    if (partySlots[ii].characterSheet)
                    {
                        if (partySlots[ii].characterSheet.unitName == missionSlot.mission.charactersRequired[i])
                        {
                            bodyFound = true;
                            break;
                        }
                    }
                }
                if (!bodyFound)
                {
                    gameManager.printError("bring required character",2.5f);
                    return;
                }
            }
            List<CharacterSheet> tempCharacterSheets = new List<CharacterSheet>();
            for (int i = 0; i < partySlots.Count; i++)
            {
                if (partySlots[i].characterSheet)
                    tempCharacterSheets.Add(partySlots[i].characterSheet);
            }
            if (tempCharacterSheets.Count == 0)
            {
                gameManager.printError("bring anycharacter", .5f);
                return;
            }
            gameManager.charactersToLoad = tempCharacterSheets;
            gameManager.startLoad(missionSlot.mission.sceneIndex);
            gameManager.currentMission = missionSlot.mission;
        }
    }
    /*
    public void levelLoaded(Scene scene, LoadSceneMode mode)
    {
        gameManager.playerTurn.myParty = FindObjectsOfType<PrimaryCharacter>().ToList();
        gameManager.playerTurn.hasLoaded = false;
        gameManager.combatManager.hasLoaded = false;
        gameManager.combatManager.newMap();
        loadingLevel = true;
        SceneManager.sceneLoaded -= levelLoaded;
    }
    public void loadCharacters()
    {
        int i = 0;
        
        foreach (PrimaryCharacter character in gameManager.combatManager.primaryCharacters)
        {
            if (!partySlots[i].characterSheet)
            {
                Destroy(character.gameObject);
            }
            else
            {
                character.loadCharacterSheet(partySlots[i].characterSheet);
            }
            i++;

        }
        gameManager.playerTurn.changeCharacter(0);
        //gameManager.playerTurn.resetDistanceMove();
    }
    */
}
