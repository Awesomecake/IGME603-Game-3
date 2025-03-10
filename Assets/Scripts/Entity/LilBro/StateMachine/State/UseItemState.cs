using UnityEngine;
using UnityEngine.Serialization;

public class UseItemState : State
{
    [SerializeField] private LilBro self;
    [SerializeField] private AbstractTargetContainer fleeTarget;

    public override void EnterState()
    {
        var itemPrefab = self.GetHeldItemPrefab();
        if (!itemPrefab) return;

        var currentLocation = self.transform.position;
        var item = Instantiate(itemPrefab, currentLocation, self.transform.rotation);
        item.layer = LayerMask.NameToLayer("Default");
        self.ClearHeldItem();
        
        var throwable = item.GetComponent<Throwable>();
        if (!throwable) return;

        var direction = currentLocation - fleeTarget.GetLocation();

        throwable.ThrowItem(500f, direction, self.gameObject);
        throwable.transform.up = direction;
    }
}