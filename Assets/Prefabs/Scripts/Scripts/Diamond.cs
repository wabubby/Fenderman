using System;
using UnityEngine;

public class Diamond : MonoBehaviour
{
    [SerializeField]
    private GameObject diamondPrefab;

    private static int count;
    // Array of predefined spawn positions
    [SerializeField]
    private Vector3[] spawnPositions = new Vector3[]
    {
        new Vector3(28, -3, -79),
        new Vector3(-145, -3, -57),
        new Vector3(-160, -5, -125),
        new Vector3(-55, 0, 30),
        new Vector3(-89, -4, -44),
        new Vector3(-75, 2, 33),
        new Vector3(-178, -3, 37)
    };

    private void OnTriggerEnter(Collider other)
    {
        PlayerInventory playerInventory = other.GetComponentInParent<PlayerInventory>(); // check if collison is with char

        if (playerInventory != null)
        {
            gameObject.SetActive(false);
            count ++;
            playerInventory.DiamondCollected();
            Spawn(count);
        }
    }
    private void Spawn(int count)
    {
        // Increment count and check if within spawn positions array
        if (count <= spawnPositions.Length)
        {
            GameObject clone = Instantiate(diamondPrefab, spawnPositions[count], Quaternion.identity);
            clone.SetActive(true);
            Debug.Log(count);
        }
    }
}