using System.Collections.Generic;
using UnityEngine;

public class EnemyDetector : AbstractTargetContainer
{
    private readonly HashSet<Enemy> _enemies = new();
    private Vector3 _fleeTarget = new Vector3();

    [SerializeField] private EventTrigger trigger;
    [SerializeField] private EventTrigger clearTrigger;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy")) return;
        var enemy = other.GetComponent<Enemy>();
        if (!enemy) return;
        _enemies.Add(enemy);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy")) return;
        var enemy = other.GetComponent<Enemy>();
        if (!enemy) return;
        _enemies.Remove(enemy);
    }

    private void FixedUpdate()
    {
        var averageEnemyPosition = new Vector3();
        var enemyCount = 0;
        foreach (var enemy in _enemies)
        {
            if (enemy.GetState() != Enemy.State.Chasing) continue;
            averageEnemyPosition += enemy.transform.position;
            enemyCount++;
        }

        if (enemyCount == 0)
        {
            clearTrigger.TriggerEvent();
            return;
        }

        averageEnemyPosition /= enemyCount;
        var enemyDirection = (averageEnemyPosition - transform.position).normalized;
        _fleeTarget = transform.position - enemyDirection - enemyDirection;
        var counter = 0;
        while (WorldManager.Instance.GetTile(_fleeTarget) != null &&
               counter < 10)
        {
            _fleeTarget -= enemyDirection;
            counter++;
        }

        trigger.TriggerEvent();
    }

    public override Vector3 GetLocation()
    {
        return _fleeTarget;
    }
}