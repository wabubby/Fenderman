using UnityEngine;
using UnityEngine.Events;
public class PlayerInventory : MonoBehaviour
{
    public int NumberOfDiamonds { get; private set; } // all scripts can read the value,
                                                      // but only this script can set the value
    public UnityEvent<PlayerInventory> OnDiamondCollected;
    public void DiamondCollected()
    {
        NumberOfDiamonds++;
        OnDiamondCollected.Invoke(this);
    }
}
