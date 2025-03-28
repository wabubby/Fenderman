using UnityEngine;

public class Diamond : MonoBehaviour
{
    [SerializeField]
    private GameObject diamondPrefab;
    private void OnTriggerEnter(Collider other) 
    {
        PlayerInventory playerInventory = other.GetComponentInParent<PlayerInventory>(); // check if collison is with char

        if (playerInventory != null)
        {
            playerInventory.DiamondCollected();
            gameObject.SetActive(false);
            Spawn();
        }
    }

    private void Update()
    {
        //if (gameObject == false)
        //{
        //    Spawn();
        //}

    }

    private void Spawn()
    {
        Vector3 randomSpawnPosition = new Vector3(Random.Range(-10, -50), -124, Random.Range(-20, -28));
        GameObject clone = Instantiate(diamondPrefab, randomSpawnPosition, Quaternion.identity);
        clone.SetActive(true);
    }

    //private void DiamondView(int amount)
    //{
    //    if (amount == 0)
    //    {
    //        diamondView = 0;
    //    }
    //}

    //private bool ShouldSpawn()
    //{
    //    if (diamondView == 0)
    //    {
    //        return true;
    //    }
    //    return false;
    //}
}
