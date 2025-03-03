using System.Collections.Generic;
using UnityEngine;

public class EnemyDetector : AbstractTargetContainer
{
    private readonly HashSet<Enemy> _enemies = new();
    private Vector3 _fleeTarget = new Vector3();

    [SerializeField] private EventTrigger trigger;

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

    private void Update()
    {
        var averageEnemyPosition = new Vector3();
        var enemyCount = 0;
        foreach (var enemy in _enemies)
        {
            if (enemy.GetState() != Enemy.State.Chasing) continue;
            averageEnemyPosition += enemy.transform.position;
            enemyCount++;
        }

        if (enemyCount == 0) return;
        averageEnemyPosition /= enemyCount;
        var enemyDirection = averageEnemyPosition - transform.position;
        _fleeTarget = -enemyDirection + transform.position;
    }

    public override Vector3 GetLocation()
    {
        return _fleeTarget;
    }
}