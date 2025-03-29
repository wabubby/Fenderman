using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    private int currentHealth = 3;
    
    public int CurrentHealth 
    {
        get { return currentHealth; }
        private set 
        {
            currentHealth = Mathf.Max(0, value); // Ensure health doesn't go below 0
            UpdateHealthDisplay();
        }
    }

    [SerializeField] private GameObject[] hearts; 

    // Damage control
    public void TakeDamage(int damageAmount = 1)
    {
        if (CurrentHealth <= 0) return; // Already dead
        
        CurrentHealth -= damageAmount; 
        
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    // Health Display
    private void UpdateHealthDisplay()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].SetActive(i < CurrentHealth);
        }
    }

    // Death
    private void Die()
    {
        Debug.Log("You died!");
        SceneManager.LoadScene("LoseScreen");
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("Enemy")) 
        {
            TakeDamage();
        }
    }
}
