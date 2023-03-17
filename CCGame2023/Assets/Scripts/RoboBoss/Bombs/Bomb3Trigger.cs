using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb3Trigger : MonoBehaviour
{
    [SerializeField] int bombDmg;
    
    float dmgTimer;
    [SerializeField] float dmgCooldown;
    bool hit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(hit) {
            dmgTimer += Time.deltaTime;
        }

        if(dmgTimer >= dmgCooldown) {
            hit = false;
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        if(col.gameObject.CompareTag("Player") && !hit) {   //if collided with player
            //deal damage
            col.gameObject.GetComponent<PlayerHealth>().TakeDamage(bombDmg);
            dmgTimer = 0;
            hit = true;
        }
    }
}
