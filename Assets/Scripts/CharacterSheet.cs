using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterSheet : ScriptableObject
{
    public Sprite myImage;
    public bool missionAble;
    public string unitName;
    public float intelligence, strength, HP, MaxHP, dexterity, constitution, wisdom, charisma, maxStamina, stamina;
    public int time, maxTime;
    public int entropy;
    public List<GameObject> passivePrefabs = new List<GameObject>();
    public List<string> CharacterRelation = new List<string>();
    public List<int> CharacterRelationScore = new List<int>();
    public List<Ability> myAbilities = new List<Ability>();
    public List<GameObject> storyBaggage;


    public void OnDestroy()
    {

    }
    public CharacterSheet CopyCharacterSheet() {
        CharacterSheet copied = CreateInstance<CharacterSheet>();
        copied.myImage = myImage;
        copied.unitName = unitName;
        copied.intelligence = intelligence;
        copied.strength = strength;
        copied.HP = HP;
        copied.MaxHP = MaxHP;
        copied.dexterity = dexterity;
        copied.constitution = constitution;
        copied.wisdom = wisdom;
        copied.charisma = charisma;
        copied.maxStamina = maxStamina;
        copied.stamina = stamina;
        copied.maxTime = maxTime;
        copied.time = time;
        copied.entropy = entropy;
        copied.missionAble = missionAble;
        copied.CharacterRelation = CharacterRelation;
        copied.CharacterRelationScore = CharacterRelationScore;
        copied.myAbilities = myAbilities;
        copied.passivePrefabs = passivePrefabs;
        return copied;
    }
}
