using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class SaveGame : MonoBehaviour
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


    //ASSUMES YOU ARE SAVING IN BASE might add addtional functionalityLater
    public void saveGame(string path) {
        if (path.Length!=0)
        {
            string newPath = "Assets\\" + path;
            StringBuilder builder = new StringBuilder("version:");
            builder.Capacity = 16;
            builder.Append(gameManager.VersionNumber);
            builder.Append(HugeSectionDivider);
            builder.Append(gameManager.characterPool.Count);
            builder.Append(HugeSectionDivider);
            foreach (CharacterSheet character in gameManager.characterPool)
            {
                if (character)
                {
                    saveCharacter(builder, character);
                }
                builder.Append(HugeSectionDivider);
            }
            foreach (bool flag in gameManager.player.storyFlags) {
                if (flag)
                {
                    builder.Append(1);
                }
                else {
                    builder.Append(0);
                }
            }
            builder.Append(HugeSectionDivider);
            using (StreamWriter sw = new StreamWriter(newPath, false, Encoding.UTF8, 65536))
            {
                sw.WriteLine(builder);
            }
        }
    }
    public void saveCharacter(StringBuilder builder, CharacterSheet characterSheet) {
        builder.Append(characterSheet.unitName);
            builder.Append(BigSectionDivider);

        builder.Append(characterSheet.missionAble);
            builder.Append(MediumSectionDivider);

        builder.Append(characterSheet.intelligence);
            builder.Append(MediumSectionDivider);

        builder.Append(characterSheet.strength);
            builder.Append(MediumSectionDivider);

        builder.Append(characterSheet.HP);
            builder.Append(MediumSectionDivider);

        builder.Append(characterSheet.MaxHP);
            builder.Append(MediumSectionDivider);

        builder.Append(characterSheet.dexterity);
            builder.Append(MediumSectionDivider);

        builder.Append(characterSheet.constitution);
            builder.Append(MediumSectionDivider);

        builder.Append(characterSheet.wisdom);
            builder.Append(MediumSectionDivider);

        builder.Append(characterSheet.charisma);
            builder.Append(MediumSectionDivider);

        builder.Append(characterSheet.maxStamina);
            builder.Append(MediumSectionDivider);

        builder.Append(characterSheet.stamina);
            builder.Append(MediumSectionDivider);

        builder.Append(characterSheet.maxTime);
            builder.Append(MediumSectionDivider);

        builder.Append(characterSheet.entropy);
            builder.Append(BigSectionDivider);
        for (int i1 = 0; i1 < characterSheet.passivePrefabs.Count; i1++)
        {
            GameObject i = characterSheet.passivePrefabs[i1];
            savePassive(builder, i.GetComponent<Passive>());
            builder.Append(MediumSectionDivider);
        }
        builder.Append(BigSectionDivider);
        if (characterSheet.myAbilities.Count != 0)
        {
            foreach (Ability i in characterSheet.myAbilities)
            {
                saveAbility(builder, i);
                builder.Append(MediumSectionDivider);
            }
        }
        builder.Append(BigSectionDivider);
        saveRelation(builder,characterSheet);
    }
    public void saveRelation(StringBuilder builder, CharacterSheet characterSheet)
    {
        int j = 0;
        foreach(string i in characterSheet.CharacterRelation) {
            builder.Append(i);
            builder.Append(smallSectionDivider);
            builder.Append(characterSheet.CharacterRelationScore[j]);
            builder.Append(MediumSectionDivider);
            j++;
        }
    }
    public void saveAbility(StringBuilder builder, Ability ability)
    {
        builder.Append(ability.abilityName);
        builder.Append(smallSectionDivider);
        builder.Append(ability.timeCost);
    }
    public void savePassive(StringBuilder builder, Passive passive)
    {
        builder.Append(passive.name);
    }
    /*
    public void loadGame(string path)
    {
        if (path.Length != 0)
        {
            StreamReader myReader = new StreamReader("Assets\\" + path);
            string reading = myReader.ReadLine();
            int i = 0;
            while (!myReader.EndOfStream)
            {
                reading = myReader.ReadLine();
            }
            myReader.Close();
            Destroy(this.gameObject);
        }
        Debug.Log("emptyPath");
    }
    public void loadCharacter()
    {


    }
    */
}
