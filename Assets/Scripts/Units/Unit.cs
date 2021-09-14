using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public enum abilityStat
{
 intelligence,
 strength,
 dexterity,
 constitution,
 wisdom,
 charisma
}
public class Unit : MonoBehaviour, IPointerDownHandler
{
    public string unitName;
    public int CharacterTime;
    public float stamina;
    public int maxTime;
    public List<Ability> myAbilities;
    public Hex myHex;
    public List<Hex> path;
    public Interactable myInteract;
    public float intelligence;
    public float strength;
    public float maxHp;
    public float hp;
    public float dexterity;
    public float constitution;
    public float wisdom;
    public float charisma;
    public float maxStamina;

    public List<GameObject> storyBaggage;
    public virtual int time
    {
        get => CharacterTime; set
        {
            if (time < value)
            {
                OnSpendTime?.Invoke(this, value);
            }
; CharacterTime = value;
        }
    }
    public virtual float Intelligence { get => intelligence; set => intelligence = value; }
    public virtual float Strength { get => strength; set => strength = value; }
    public virtual float MaxHp { get => maxHp; set => maxHp = value; }
    public virtual float Hp { get => hp; set => hp = value; }
    public virtual float Dexterity { get => dexterity; set => dexterity = value; }
    public virtual float Constitution { get => constitution; set => constitution = value; }
    public virtual float Wisdom { get => wisdom; set => wisdom = value; }
    public virtual float Charisma { get => charisma; set => charisma = value; }
    public virtual float MaxStamina { get => maxStamina; set => maxStamina = value; }
    public virtual float Stamina { get => stamina; set => stamina = value; }


    public static event Action<Unit, Unit> OnAdjacentToGrunt;
    public static event Action<Unit, Unit> OnAdjacentToNeutralUnit;
    public static event Action<Unit, Unit> OnAdjacentToPrimaryCharacter;
    public static event Action<Unit, Unit> OnAdjacentToEnemy;

    public static event Action<Unit> onMoveSquare;
    public static event Action<Unit> onUnitDespawn;
    public static event Action<Unit> onClick;
    public static event Action<Unit, abilityStat, int> OnSkillCheck;
    public static event Action<Unit, int> OnSpendTime;
    public static event Action<Unit> onKillUnit;
    public static event Action<Unit, float, Unit> onTakeDamage;
    public static event Action<Unit, float> onHeal;
    public static event Action<Unit, float> onCritcalHealth;
    public virtual void Start()
    {

    }
    public virtual void takeDamage(int damage, Unit dealer)
    {
        onTakeDamage?.Invoke(this, damage, dealer);
        Hp -= damage;
        if (Hp <= 0)
        {
            onKillUnit?.Invoke(dealer);
            die();
        }
    }
    public virtual void takeDamage(float damage, Unit dealer)
    {
        onTakeDamage?.Invoke(this, damage, dealer);
        Hp -= damage;
        if (Hp <= 1 / 3 * MaxHp)
        {
            onCritcalHealth?.Invoke(this, Hp);
        }
        if (Hp <= 0)
        {
            die();
        }
    }

    public virtual void healDamage(int damage)
    {

        onHeal?.Invoke(this, damage);
        Hp += damage;
        if (Hp <= 0)
        {
            die();
        }
    }
    public virtual void die()
    {
        myHex.myUnit = null;
        Destroy(this.gameObject);

    }
    public virtual void Update()
    {
        if (!myHex)
        {
            RaycastHit hit;
            Physics.Raycast(transform.position, new Vector3(0, -1, 0), out hit);
            if (hit.collider.gameObject.TryGetComponent(out Hex myNewHex))
            {
                myHex = myNewHex;
                myHex.myUnit = this;
                this.transform.position = myHex.transform.position + Vector3.up * .4f;
            }
        }
    }
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        onClick?.Invoke(this);
    }

    public virtual void OnDestroy()
    {
        onUnitDespawn?.Invoke(this);
        myHex.myUnit = null;
    }
    public bool skillCheck(abilityStat type, int skillType)
    {
        OnSkillCheck?.Invoke(this, type, skillType);
        return true;
    }
    public void moveIntoSquare()
    {
        onMoveSquare?.Invoke(this);
        if (myHex)
        {
            foreach (Hex i in myHex.adjacentHexes)
            {
                if (i)
                {
                    if (i.myUnit)
                    {
                        if (i.myUnit is PrimaryCharacter)
                        {
                            OnAdjacentToPrimaryCharacter?.Invoke(this, i.myUnit);
                        }
                        else if (i.myUnit is SecondaryCharacter)
                        {
                            OnAdjacentToGrunt?.Invoke(this, i.myUnit);
                        }
                        else if (i.myUnit is Enemy)
                        {
                            OnAdjacentToEnemy?.Invoke(this, i.myUnit);
                        }
                        else
                        {
                            OnAdjacentToNeutralUnit?.Invoke(this, i.myUnit);
                        }

                        if (this is PrimaryCharacter)
                        {
                            OnAdjacentToPrimaryCharacter?.Invoke(i.myUnit, this);
                        }
                        else if (this is SecondaryCharacter)
                        {
                            OnAdjacentToGrunt?.Invoke(i.myUnit, this);
                        }
                        else if (this is Enemy)
                        {
                            OnAdjacentToEnemy?.Invoke(i.myUnit, this);
                        }
                    }
                }
            }
        }
    }
}
