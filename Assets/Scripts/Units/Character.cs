using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class Character : Unit
{
    public int Entropy;
    public StatBar myHPBar;
    public StatBar myTimeBar;
    public StatBar myEntropyBar;
    public Image myImage;
    public static event Action<Unit,int> OnEntropyChange;
    public new static event Action<Unit, int> OnSpendTime;
    public override void Start()
    {
        base.Start();
        updateBars();

    }
    public void updateBars() {
        myTimeBar.UpdateBar(time, maxTime);
        myHPBar.UpdateBar(Hp, MaxHp);
        myEntropyBar.UpdateBar(entropy, 100);

    }
    public override int time { get => CharacterTime; set
        {
            if (time < value)
            {
                OnSpendTime?.Invoke(this, value);
            }
            CharacterTime = value;
            setTimeBar(value);
        }
    }
    public virtual int entropy
    {
        get => Entropy; set
        {
            OnEntropyChange?.Invoke(this, value);
            Entropy = value;
            
            myEntropyBar.UpdateBar(Entropy, 100);
        }
    }

    public void readyCombat() {
        time = maxTime;
        myHPBar.UpdateBar(Hp,MaxHp);
    
    }
    public override void takeDamage(int damage, Unit dealer)
    {
        base.takeDamage(damage, dealer);
        myHPBar.UpdateBar(Hp,MaxHp);
    }
    public override void healDamage(int damage)
    {
        base.healDamage(damage);
        myHPBar.UpdateBar(Hp, MaxHp);
    }
    public void setTimeBar(int time) {
        myTimeBar.UpdateBar(time, maxTime);
    }
}