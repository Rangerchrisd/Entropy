using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyTurn : MonoBehaviour
{
    public GameManager gameManager;
    public Hex target;

    public int enemyOn = 0;

    public static event Action onStartTurn;
    public static event Action onEndTurn;
    public void newRound()
    {
        onStartTurn?.Invoke();
        enemyOn = 0;
    }
    public void Update()
    {
        if (gameManager.combatManager.activeTurn == turnType.Enemy) {

            if (gameManager.combatManager.enemies.Count > enemyOn)
            {
                if (!gameManager.combatManager.enemies[enemyOn].activeTurn)
                    doEnemyUnitTurn(gameManager.combatManager.enemies[enemyOn]);
            }
            else
            {
                onEndTurn?.Invoke();
                gameManager.combatManager.TurnEnded(turnType.Enemy);
            }
        }
    }
    public void doEnemyUnitTurn(Enemy enemy) {
        enemy.startTurn();
    }
}
