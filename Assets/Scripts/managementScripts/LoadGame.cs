using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoadGame : MonoBehaviour
{
    public GameManager gameManager;
    public bool showDividers = false;
    [ConditionalHide("showDividers", true)]
    public char HugeSectionDivider = '\n';
    [ConditionalHide("showDividers", true)]
    public char BigSectionDivider = '&';
    [ConditionalHide("showDividers", true)]
    public char MediumSectionDivider = '~';
    [ConditionalHide("showDividers", true)]
    public char smallSectionDivider = '`';
    [ConditionalHide("showDividers", true)]
    public char tinySectionDivider = '$';
    //breaks if medium is a .
    int versionLoading;

    //ASSUMES YOU ARE SAVING IN BASE might add addtional functionalityLater
    public void loadGame(string path)
    {
        if (path.Length != 0)
        {
            string newPath = "Assets\\" + path;
            StreamReader streamReader = new StreamReader(newPath);
            string readLine =  streamReader.ReadLine();
            readLine = readLine.Substring(readLine.IndexOf(':')+1);
            versionLoading = int.Parse(readLine);
            if (gameManager.VersionNumber != versionLoading) {
                Debug.Log("LoadingDifferentVersion");
            }
            readLine = streamReader.ReadLine();
            int characterCount = int.Parse(readLine);

            for (int i = 0; i < characterCount; i++) {
                loadCharacter(streamReader.ReadLine());
            }
            readLine = streamReader.ReadLine();
            for (int i = 0; i < readLine.Length; i++)
            {
                if (int.Parse(readLine[i].ToString()) == 1)
                {
                    gameManager.player.storyFlags[i] = true;
                }
                else {
                    gameManager.player.storyFlags[i] = false;
                }
            }
            loadCharacters();
        }
    }
    public void loadCharacter(string characterInfo)
    {
        string unitName = "";
        unitName = characterInfo.Substring(0, characterInfo.IndexOf(BigSectionDivider));
        int indexLook = 0;
        CharacterSheet characterFound = null;
        foreach (CharacterSheet character in gameManager.characterPool)
        {
            if (unitName == character.unitName) {
                characterFound = character;
                break;
            }
            indexLook++;
        }
        if (characterFound)
        {

            gameManager.characterPool[indexLook] = characterFound.CopyCharacterSheet();
            CharacterSheet current = gameManager.characterPool[indexLook];
            characterInfo = characterInfo.Substring(characterInfo.IndexOf(BigSectionDivider)+1);

           
            int indexOFValue= characterInfo.IndexOf(MediumSectionDivider);
            current.missionAble = (characterInfo.Substring(0, indexOFValue) == "True");
            characterInfo = characterInfo.Substring(indexOFValue+1);

            indexOFValue = characterInfo.IndexOf(MediumSectionDivider);
            current.intelligence = float.Parse(characterInfo.Substring(0, indexOFValue));
            characterInfo = characterInfo.Substring(indexOFValue+1);

            indexOFValue = characterInfo.IndexOf(MediumSectionDivider);
            current.strength = float.Parse(characterInfo.Substring(0, indexOFValue));
            characterInfo = characterInfo.Substring(indexOFValue+1);

            indexOFValue = characterInfo.IndexOf(MediumSectionDivider);
            current.HP = float.Parse(characterInfo.Substring(0, indexOFValue));
            characterInfo = characterInfo.Substring(indexOFValue+1);

            indexOFValue = characterInfo.IndexOf(MediumSectionDivider);
            current.MaxHP = float.Parse(characterInfo.Substring(0, indexOFValue));
            characterInfo = characterInfo.Substring(indexOFValue+1);

            indexOFValue = characterInfo.IndexOf(MediumSectionDivider);
            current.dexterity = float.Parse(characterInfo.Substring(0, indexOFValue));
            characterInfo = characterInfo.Substring(indexOFValue+1);

            indexOFValue = characterInfo.IndexOf(MediumSectionDivider);
            current.constitution = float.Parse(characterInfo.Substring(0, indexOFValue));
            characterInfo = characterInfo.Substring(indexOFValue+1);

            indexOFValue = characterInfo.IndexOf(MediumSectionDivider);
            current.wisdom = float.Parse(characterInfo.Substring(0, indexOFValue));
            characterInfo = characterInfo.Substring(indexOFValue+1);

            indexOFValue = characterInfo.IndexOf(MediumSectionDivider);
            current.charisma = float.Parse(characterInfo.Substring(0, indexOFValue));
            characterInfo = characterInfo.Substring(indexOFValue+1);

            indexOFValue = characterInfo.IndexOf(MediumSectionDivider);
            current.maxStamina = float.Parse(characterInfo.Substring(0, indexOFValue));
            characterInfo = characterInfo.Substring(indexOFValue + 1);

            indexOFValue = characterInfo.IndexOf(MediumSectionDivider);
            current.stamina = float.Parse(characterInfo.Substring(0, indexOFValue));
            characterInfo = characterInfo.Substring(indexOFValue + 1);

            indexOFValue = characterInfo.IndexOf(MediumSectionDivider);
            current.maxTime = int.Parse(characterInfo.Substring(0, indexOFValue));
            characterInfo = characterInfo.Substring(indexOFValue + 1);

            indexOFValue = characterInfo.IndexOf(BigSectionDivider);
            current.entropy = int.Parse(characterInfo.Substring(0, indexOFValue));
            characterInfo = characterInfo.Substring(indexOFValue+1);


            indexOFValue = characterInfo.IndexOf(BigSectionDivider);
            //passive
            characterInfo = characterInfo.Substring(indexOFValue + 1);

            indexOFValue = characterInfo.IndexOf(BigSectionDivider);
            loadAbilities(characterInfo.Substring(0, indexOFValue), current);
            characterInfo = characterInfo.Substring(indexOFValue + 1);


        }
        else {
            Debug.LogError("Save fault");
        
        }
    }

    public void loadAbilities(string abilities, CharacterSheet current)
    {
        int indexOfValue = 0;
        while (abilities != "")
        {
            //Debug.Log("what's left?" + abilities);
            indexOfValue = abilities.IndexOf(MediumSectionDivider);
            loadAbility(abilities.Substring(0, indexOfValue),current);
            abilities = abilities.Substring(indexOfValue + 1);
        }
    }
    public void loadAbility(string ability, CharacterSheet current)
    {
        int indexOfValue = 0;
        indexOfValue = ability.IndexOf(smallSectionDivider);
        Ability abilityFound = null;
        string currentAbility = ability.Substring(0, indexOfValue);
        foreach (Ability i in current.myAbilities) {
            if (i.abilityName == currentAbility) {
                abilityFound = i;
                break;
            }
        }
        if (abilityFound) {
            ability = ability.Substring(indexOfValue + 1);

        }
    }

    public void loadCharacters()
    {
        int i = 0;

        foreach (PrimaryCharacter character in gameManager.combatManager.primaryCharacters)
        {
            character.loadCharacterSheet(gameManager.characterPool[i]);//.CopyCharacterSheet());
            i++;
        }
        
        gameManager.playerTurn.changeCharacter(0);
    }
}
