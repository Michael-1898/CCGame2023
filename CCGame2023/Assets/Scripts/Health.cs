using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    int currentHealth;
    public bool hit;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(hit != false) {
            hit = false;
        }
    }

    public void TakeDamage(int dmg) {
        hit = true;
        currentHealth -= dmg;

        if(currentHealth <= 0) {
            Die();
        }
    }

    void Die() {
        Destroy(gameObject);
    }
}
