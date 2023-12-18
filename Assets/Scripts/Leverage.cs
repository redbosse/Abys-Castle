using UnityEngine;
using UnityEngine.Events;

public class Leverage : GrabableItem
{
    [SerializeField]
    private UnityEvent LeverageEvent;

    public override GameObject Grab()
    {
        LeverageEvent?.Invoke();
        return this.gameObject;
    }
}