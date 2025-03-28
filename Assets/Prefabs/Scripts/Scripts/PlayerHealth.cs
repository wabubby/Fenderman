
using System;
using UnityEngine;
using UnityEngine.UI;


public class PlayerHealth : MonoBehaviour
{

    public int maxHealth = 3;
    [SerializeField] GameObject[] hearts;
    public int currentHealth;
    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Enemy")) {
            if (currentHealth >= 1){
                Destroy(hearts[currentHealth-1]);
                currentHealth -= 1;
            }
            if(currentHealth <= 1){
                Debug.Log("you died");
                // Switch Scene
                return;
            }
         
        }
    }

} 
