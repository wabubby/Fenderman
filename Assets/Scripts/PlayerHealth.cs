
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;
    void Start()
    {
        currentHealth = maxHealth;
    }

    void TakeDamage(int damage) {
        if (currentHealth > 0) {
            currentHealth -= damage;
        }
        // anim.SetBool("IsDead", true);
    }
} 
