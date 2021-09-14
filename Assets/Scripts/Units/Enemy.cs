using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class Enemy : Unit, IPointerClickHandler
{
    public List<Hex> hexTargets;
    public StatBar myHPBar;
    public static event Action resetPathing;
    public static event Action<Enemy> enemyDie;
    public static event Action<Enemy> enemyDespawn;
    public static event Action<Enemy> spawnedEnemy;
    public static event Action<Unit> OnClick;
    public bool activeTurn, isMoving, fullDistance;
    public float animationTime = 4;
    public float currentTime, unitHeightOffset = .4f;
    public int attackCost;

    public CombatManager combatManager;
    public GameManager gameManager;
    public Hex target;
    public virtual void startTurn() {
        activeTurn = true;
        isMoving = true;
        time = maxTime;
    }
    public virtual void endTurn()
    {
        target = null;
        activeTurn = false;
        isMoving = false;
        fullDistance = false;
        combatManager.gameManager.enemyPlayerTurn.enemyOn++;
    }
    public override void Update()
    {
        base.Update();
        if (activeTurn) {
            if (currentTime<=0)
            {
                if (isMoving && path.Count == 0)
                {
                    //Reset Pathing
                    resetDistanceMove();
                    chooseTarget();
                    readyMove();
                } else if (isMoving) {
                    moveSquare();
                } else if (!isMoving) {
                    doAction();
                    endTurn();
                }
            }
            else {
                currentTime -= gameManager.playerSettings.gameSpeed * Time.deltaTime;
            }
        }
    }
    public virtual void chooseTarget() {
        target =  combatManager.findClosestAlly();
        if (target.distanceTo <= (time + attackCost)) {
            fullDistance = false;
            path = target.copyHexPath();
            if (path.Count == 1) {
                Debug.Log("only one space left");
            }
        }
        else {
            while (target.distanceTo > time)
            {
                target = target.Path[target.Path.Count - 1];
            }
            path = target.Path[target.Path.Count - 1].copyHexPath();
            fullDistance = true;
        }
    }
    public void moveSquare()
    {
        // if in combat use player time
        CharacterTime -= 1;
        //give animation time
        currentTime = 3;
        //get next square
        Hex nextStepHex = path[0];
        //having gone there you don't need to go there anymore
        path.RemoveAt(0);
        //change which hex you are on
        myHex.myUnit = null;
        myHex = nextStepHex;
        //check for square effects
        nextStepHex.enterHex(this);
        //set new hex
        nextStepHex.myUnit = this;
        //Sets character model at correct height
        transform.position = nextStepHex.transform.position + Vector3.up * unitHeightOffset;
        //if you are finished moving, set move to 0
        if (path.Count == 0)
        {
            //set animation time to done
            currentTime = .5f;
            //if you were interacting with a square, interact with it
            isMoving = false;
        }
        moveIntoSquare();
    }
    public void resetDistanceMove()
    {
        resetPathing?.Invoke();
        myHex.findDistance();

    }
    public void readyMove() {
        //remove own square
        path.RemoveAt(0);
        //should remove the final step unless you are in the square
        if (path.Count > 0)
        {
            path.RemoveAt(path.Count - 1);
        }
        if (path.Count == 0)
        {
            isMoving = false;
            currentTime = .5f;
        }
    }
    public virtual void doAction() {
        if (target.myUnit) {
            time -= attackCost;
            target.myUnit.takeDamage(Strength, this);
        }
    }
    public override void die() {
        enemyDie?.Invoke(this);
        base.die();
    }
    public override void Start()
    {
        base.Start();
        combatManager = FindObjectOfType<CombatManager>();
        gameManager = combatManager.gameManager;
        spawnedEnemy?.Invoke(this);
        myHPBar.UpdateBar(Hp, MaxHp);
        //PlayerTurnManager.findEnemiesEvent += foundEnemy;
    }
    public override void OnDestroy()
    {
        base.OnDestroy();
        //PlayerTurnManager.findEnemiesEvent -= foundEnemy;
    }
    public override void takeDamage(int damage, Unit dealer)
    {
        base.takeDamage(damage, dealer);
        myHPBar.UpdateBar(Hp, MaxHp);
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke(this);
    }
}
