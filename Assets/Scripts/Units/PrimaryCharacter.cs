using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;


public class PrimaryCharacter : Character
{
    public bool hasLoaded;
    public CharacterSheet myCharacter;
    public List<GameObject> passiveObjects;
    public static event Action<PrimaryCharacter> primaryStatChange;
    public static event Action<PrimaryCharacter> primaryDie;
    public static event Action<PrimaryCharacter> primaryDespawn;
    public static event Action<PrimaryCharacter> spawnPrimary;
    public static event Action<PrimaryCharacter> updateTImeBar;
    public static event Action<PrimaryCharacter> updateHealthBar;
    public static event Action<PrimaryCharacter> updateEntropyBar;
    public static event Action<Unit, int> spendTime;

    public override float Strength { get => strength; set { strength = value; primaryStatChange?.Invoke(this); } }
    public override float MaxHp { get => maxHp; set { maxHp = value; primaryStatChange?.Invoke(this); } }
    public override float Hp { get => hp; set { hp = value; primaryStatChange?.Invoke(this); } }
    public override float Dexterity { get => dexterity; set { dexterity = value; primaryStatChange?.Invoke(this); } }
    public override float Constitution { get => constitution; set { constitution = value; primaryStatChange?.Invoke(this); } }
    public override float Wisdom { get => wisdom; set { wisdom = value; primaryStatChange?.Invoke(this); } }
    public override float Charisma { get => charisma; set { charisma = value; primaryStatChange?.Invoke(this); } }
    public override float MaxStamina { get => maxStamina; set { maxStamina = value; primaryStatChange?.Invoke(this); } }
    public override float Stamina { get => stamina; set { stamina = value; primaryStatChange?.Invoke(this); } }

    public override int time
    {
        get => CharacterTime; set
        {
            if (time < value)
            {
                spendTime?.Invoke(this, value);
            }
            CharacterTime = value;
            setTimeBar(value);
            updateTImeBar?.Invoke(this);
        }
    }
    public override int entropy
    {
        get => Entropy; set
        {
            Entropy = value;
            updateEntropyBar?.Invoke(this);
            myEntropyBar.UpdateBar(Entropy, 100);
        }
    }
    public override void takeDamage(int damage, Unit dealer)
    {
        base.takeDamage(damage, dealer);
        updateHealthBar?.Invoke(this);
    }
    public override void die()
    {
        primaryDie?.Invoke(this);
        base.die();
    }
    public override void Start()
    {
        base.Start();
        myImage.sprite = myCharacter.myImage;
    }
    public override void OnDestroy()
    {
        base.OnDestroy(); 
        primaryDespawn?.Invoke(this);
    }
    public override void Update()
    {
        base.Update();
        if (!hasLoaded) {
            if (myHex) {
                hasLoaded = true;
                spawnPrimary?.Invoke(this);
            }
        }
        if (Input.GetKeyDown(KeyCode.R)) {
            entropy = 4;
        }
    }

    public void saveCharacterSheet()
    {
        if (!myCharacter)
        {
            Debug.LogError("no Charactersheet");
            return;
        }
        //invertALLtheese
        myCharacter.myImage = myImage.sprite;
        myCharacter.unitName = unitName;
        myCharacter.intelligence = Intelligence;
        myCharacter.strength = Strength;
        myCharacter.HP = Hp;
        myCharacter.MaxHP = MaxHp;
        myCharacter.dexterity = Dexterity;
        myCharacter.constitution = Constitution;
        myCharacter.wisdom = Wisdom;
        myCharacter.charisma = Charisma;
        myCharacter.dexterity = Dexterity;
        myCharacter.maxStamina = MaxStamina;
        myCharacter.stamina = Stamina;
        myCharacter.maxTime = maxTime;
        myCharacter.time = time;
        myCharacter.entropy = entropy;

        //not saving abilities or passives since those should only be changed carefully and those events should be more careful

    }

    public void loadCharacterSheet(CharacterSheet myCharacter) {
        if (!myCharacter) {
            Destroy(this.gameObject);
        }
        myImage.sprite =    myCharacter.myImage;
        unitName =          myCharacter.unitName;
        Intelligence =      myCharacter.intelligence;
        Strength =          myCharacter.strength;
        Hp =                myCharacter.HP;
        MaxHp =             myCharacter.MaxHP;
        Dexterity =         myCharacter.dexterity;
        Constitution =      myCharacter.constitution;
        Wisdom =            myCharacter.wisdom;
        Charisma =          myCharacter.charisma;
        MaxStamina  =       myCharacter.maxStamina;
        Stamina =           myCharacter.stamina;
        maxTime =           myCharacter.maxTime;
        time =              myCharacter.time;
        this.myCharacter =  myCharacter;
        myAbilities =       myCharacter.myAbilities;
        entropy =           myCharacter.entropy;
        foreach (GameObject i in passiveObjects) {
            Destroy(i);
        }
        foreach (GameObject passivePrefab in myCharacter.passivePrefabs) {
            GameObject newPassive  = Instantiate(passivePrefab);
            newPassive.GetComponent<Passive>().myCharacter = this;
            newPassive.transform.SetParent(this.transform);
            passiveObjects.Add(newPassive);
        }
        /*
        if(passvie)
            passive
            */
        updateBars();
    }
}
