using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.FPS.Game;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> allEnemies = new List<GameObject>();

    public void AddEnemy(GameObject enemy)
    {
        if (enemy != null && !allEnemies.Contains(enemy))
        {
            allEnemies.Add(enemy);
        }
    }

    public void RemoveEnemy(GameObject enemy)
    {
        if (enemy != null && allEnemies.Contains(enemy))
        {
            allEnemies.Remove(enemy);

            // create an event, pass in the enemy that died, and the remaining enemies.
            EnemyKillEvent deathEvent = new EnemyKillEvent();
            deathEvent.Enemy = enemy;
            deathEvent.RemainingEnemyCount = allEnemies.Count;

            // send out the message an enemy has died so our UI get's updated
            EventManager.Broadcast(deathEvent);
        }
    }
}
