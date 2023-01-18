using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    int currentHealth;
    public bool hit;
    int once;

    // Start is called before the first frame update
    void Start()
    {
        once = 0;
        currentHealth = maxHealth;
        hit = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(hit != false && once >= 1) {
            hit = false;
        } else {
            once++;
        }
    }

    public void TakeDamage(int dmg) {
        hit = true;
        once = 0;
        currentHealth -= dmg;

        if(currentHealth <= 0) {
            Die();
        }
    }

    void Die() {
        Destroy(gameObject);
    }
}
