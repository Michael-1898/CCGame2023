using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    //variables for health
    [SerializeField] int maxHealth;
    public int currentHealth;
    public bool hit;
    public bool invincible;
    float iFrameTimer;
    [SerializeField] float iFrameTime;
    bool iFrameActive;

    //variables for health display
    [SerializeField] Image[] hearts;
    [SerializeField] Sprite fullHeart;
    [SerializeField] Sprite emptyHeart;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        hearts[0] = GameObject.Find("Canvas").transform.GetChild(0).GetChild(0).GetComponent<Image>();
        hearts[1] = GameObject.Find("Canvas").transform.GetChild(0).GetChild(1).GetComponent<Image>();
        hearts[2] = GameObject.Find("Canvas").transform.GetChild(0).GetChild(2).GetComponent<Image>();
        hearts[3] = GameObject.Find("Canvas").transform.GetChild(0).GetChild(3).GetComponent<Image>();
        hearts[4] = GameObject.Find("Canvas").transform.GetChild(0).GetChild(4).GetComponent<Image>();
        hearts[5] = GameObject.Find("Canvas").transform.GetChild(0).GetChild(5).GetComponent<Image>();
        hearts[6] = GameObject.Find("Canvas").transform.GetChild(0).GetChild(6).GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if(hit != false) {
            hit = false;
        }

        for(int i = 0; i < hearts.Length; i++) {
            if(i < currentHealth) {
                hearts[i].sprite = fullHeart;
            } else {
                hearts[i].sprite = emptyHeart;
            }

            if(i < maxHealth) {
                hearts[i].enabled = true;
            } else {
                hearts[i].enabled = false;
            }
        }

        if(iFrameActive) {
            iFrameTimer += Time.deltaTime;
            if(iFrameTimer >= iFrameTime) {
                iFrameActive = false;
            }
        }
    }

    public void TakeDamage(int dmg) {
        if(!invincible && !iFrameActive) {
            hit = true;
            currentHealth -= dmg;

            if(currentHealth <= 0) {
                for(int i = 0; i < hearts.Length; i++) {
                    hearts[i].sprite = emptyHeart;
                }
                Die();
            }

            iFrameTimer = 0;
            iFrameActive = true;
        }
        
    }

    void Die() {
        Destroy(gameObject);
        SceneManager.LoadScene("Menu");
    }
}
