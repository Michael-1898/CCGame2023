using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardoAttack : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject fireball;
    GameObject player;
    int attackType = 0;
    float attackTimer = 0f;
    float timeBetweenAttacks = 2f;
    float timeBetweenFireballs = 0.5f;
    int fireballNumber = 5;
    int count = 0;

    public bool attacking = false;
    
    void Start()
    {
        if(GameObject.Find("Player") != null)
        {
            player = GameObject.Find("Player");
        }
        else player = GameObject.Find("Player(Clone)");
        
    }

    // Update is called once per frame
    void Update()
    {
        if(attacking == false && attackTimer > timeBetweenAttacks)
        {
            print("ATTACK!");
            attackType = Random.Range(1,3);
            attacking = true;
            attackTimer = 0f;
        }    
        Attack();
        attackTimer += Time.deltaTime;
    }

    void Attack()
    {
        
            
        if(attackType == 1)
        {
            FireballAttack();
        }
        if(attackType == 2)
        {
            print("attack 2");
            attacking = false;
        }
        if(attackType == 3)
        {
            print("attack 3");
            attacking = false;
        }
        if(attackType == 4)
        {
            print("attack 4");
            attacking = false;
        }
    }

    void FireballAttack()
    {
        if(attackTimer > timeBetweenFireballs)
        {
            Instantiate(fireball, transform.position, Quaternion.identity);
            count++;
            attackTimer = 0f;
        }
        if(count >= fireballNumber)
        {
            attacking = false;
            count = 0;
            attackType = 0;
        }
         
            
    }
}
