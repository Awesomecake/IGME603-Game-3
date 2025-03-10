using System.Collections.Generic;
using UnityEngine;

public class PlayerLineOfSightDetector : MonoBehaviour
{
    [SerializeField] private PlayerDetector playerDetector;
    
    private readonly Dictionary<Collider2D, bool> _spottedStatus = new();
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        CheckPlayer(other);
        CheckBro(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        CheckPlayer(other);
        CheckBro(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var isTargetVisible = _spottedStatus.GetValueOrDefault(other, false);
        if (!isTargetVisible) return;
        playerDetector.LostTarget(other.transform);
        _spottedStatus.Remove(other);
    }

    public void Clear()
    {
        _spottedStatus.Clear();
    }

    private void CheckPlayer(Collider2D other)
    {
        const string targetTag = "Player";
        if (!other.CompareTag(targetTag)) return;
        SpottedTarget(other, targetTag);
    }

    private void CheckBro(Collider2D other)
    {
        const string targetTag = "LilBro";
        if (!other.CompareTag(targetTag)) {return;}
        var bro = other.GetComponent<LilBro>();
        if (!bro) return;
        if (bro.GetCurrentState() == LilBro.State.Downed) return;
        SpottedTarget(other, targetTag);
    }

    private void SpottedTarget(Collider2D other, string targetTag)
    {
        var targetPosition = other.transform.position;
        var selfPosition = playerDetector.transform.position;
        
        var direction = targetPosition - selfPosition;
        var distance = selfPosition.DistanceTo2D(targetPosition);
        
        var hits = Physics2D.RaycastAll(selfPosition, direction, distance);
        foreach (var hit in hits)
        {
            if (hit.collider.CompareTag("Untagged"))
            {
                UpdateTargetBlocked(other);
                Debug.DrawLine(selfPosition, targetPosition, Color.blue);
                return;
            }

            if (hit.collider.CompareTag(targetTag))
            {
                UpdateTargetVisible(other);
                Debug.DrawLine(selfPosition, targetPosition, Color.green);
                return;
            }
        }
    }

    private void UpdateTargetVisible(Collider2D other)
    {
        var canSeeCurrently = _spottedStatus.GetValueOrDefault(other, false);
        if (!canSeeCurrently)
        {
            playerDetector.SpottedTarget(other.transform);
        }
        _spottedStatus[other] = true;
    }
    
    private void UpdateTargetBlocked(Collider2D other)
    {
        var canSeeCurrently = _spottedStatus.GetValueOrDefault(other, true);
        if (!canSeeCurrently)
        {
            playerDetector.LostTarget(other.transform);
        }
        _spottedStatus[other] = false;
    }
}