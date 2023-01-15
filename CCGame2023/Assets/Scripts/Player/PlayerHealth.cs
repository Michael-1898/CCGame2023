﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    //variables for health
    [SerializeField] private int maxHealth;
    int currentHealth;
    public bool hit;

    //variables for health display
    [SerializeField] Image[] hearts;
    [SerializeField] Sprite fullHeart;
    [SerializeField] Sprite emptyHeart;

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

        // for(int i = 0; i < hearts.length; i++) {
        //     if(i < currentHealth) {
        //         hearts[i].sprite = fullHeart;
        //     } else {
        //         hearts[i].sprite = emptyHeart;
        //     }

        //     if(i < maxHealth) {
        //         hearts[i].enabled = true;
        //     } else {
        //         hearts[i].enabled = false;
        //     }
        // }
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
