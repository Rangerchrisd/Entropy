using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum AbilityTarget
{
    None,
    Hex,
    EmptyHex,
    Enemy,
    Ally,
    Unit,
    Custom
}
[CreateAssetMenu(fileName = "Ability", menuName = "Ability/Default")]
public class Ability : ScriptableObject
{
    public bool isAttack;
    public Sprite myAbilitySprite;
    public int cooldownLeft, maxCooldown, range, timeCost;
    public Unit currentCharacter;
    public AbilityTarget abilityTarget;
    public string abilityName;
    public static event Action<Unit> onAttack;
    public static event Action<Ability> onTarget;
    public static event Action<Ability> onCancelTarget;
    public void targetAbility(PlayerTurn playerTurn) {
        if (playerTurn.targettingAbility != this)
        {
            onTarget?.Invoke(this);
            playerTurn.interruptMoving();
            playerTurn.targettingAbility = this;
            if (abilityTarget == AbilityTarget.Enemy)
            {
                targetEnemy(playerTurn);
            }
            else if (abilityTarget ==AbilityTarget.Ally)
            {
                targetAlly(playerTurn);
            }
        }
        else {
            onCancelTarget?.Invoke(this);
            playerTurn.targettingAbility = null;
            playerTurn.possibleTargets.Clear();
            playerTurn.resetDistanceMove();
            
        }
    }
    public virtual void doAbility(Hex targetHex, Unit user) {
        if (isAttack) {
            onAttack?.Invoke(user);
        }
        user.time -= timeCost;
        onCancelTarget?.Invoke(this);
    }
    public void CooldownProcess() {
        if (cooldownLeft > 0) {
            cooldownLeft--;
        }
    }
    public void targetEnemy(PlayerTurn playerTurn)
    {
        playerTurn.resetDistanceFight();
        currentCharacter = playerTurn.activeCharacter;
        playerTurn.possibleTargets = playerTurn.gameManager.combatManager.findEnemies(range);
        playerTurn.ChangeColor(playerTurn.possibleTargets,Color.red);
    }
    public void targetAlly(PlayerTurn playerTurn)
    {
        playerTurn.resetDistanceFight();
        currentCharacter = playerTurn.activeCharacter;
        playerTurn.possibleTargets = playerTurn.gameManager.combatManager.findAlliesInRange(range);
        playerTurn.ChangeColor(playerTurn.possibleTargets, Color.green);
    }
    public void outsideCancelTarget()
    {
        onCancelTarget?.Invoke(this);

    }
}
