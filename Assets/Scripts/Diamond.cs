using UnityEngine;

public class Diamond : MonoBehaviour
{

    private void OnTriggerEnter(Collider other) 
    {
        PlayerInventory playerInventroy = other.GetComponent<PlayerInventory>(); // check if collison is with char

        if (playerInventroy != null)
        {
            playerInventroy.DiamondCollected();
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {

    }
}
