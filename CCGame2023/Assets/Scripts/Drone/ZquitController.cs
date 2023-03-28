﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZquitController : MonoBehaviour
{
    //movement variables
    [SerializeField] float aggroRadius;
    [SerializeField] float followRadius;
    [SerializeField] float moveSpeed;
    [SerializeField] LayerMask playerLayer;
    Transform player;
    bool isFacingRight;
    Transform drone;

    //attk variables
    [SerializeField] float attkCooldown;
    float attkTimer;
    bool isAttacking;
    [SerializeField] float attkingSpeed;
    bool aggroTaken;
    Rigidbody2D rb;
    [SerializeField] int enemyDmg;
    bool playerHit;

    //knockbacked variables
    float hitTimer;
    [SerializeField] float stunTime;
    bool isKnocked;

    //anim variable
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        drone = transform;
        isAttacking = false;
        aggroTaken = false;
        rb = GetComponent<Rigidbody2D>();
        isFacingRight = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isAttacking && drone.localRotation.eulerAngles.z != 0) {
            transform.rotation = Quaternion.Euler(0f, drone.localRotation.eulerAngles.y, 0f);
        }

        //linear drag when knockbacked
        if(GetComponent<Health>().hit == true && rb.drag == 0) {
            rb.drag = 1.5f;
            isKnocked = true;
        }

        if(GetComponent<Health>().hit == true && isKnocked == true) {
            hitTimer = 0;
        }

        if(isKnocked) {
            hitTimer += Time.deltaTime;
        }

        if(hitTimer >= stunTime) {
            rb.drag = 0;
            rb.velocity = Vector3.zero;
            isKnocked = false;
            hitTimer = 0;
            if(isAttacking) {
                isAttacking = false;
                anim.SetBool("isAttking", false);
                transform.Rotate(new Vector3(0, 0, -20));
            }
        }


        //flipping to look at player
        if(transform.position.x < player.position.x && !isFacingRight && !isAttacking) {
            Flip();
        } else if(transform.position.x > player.position.x && isFacingRight && !isAttacking) {
            Flip();
        }


        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);

        //initial aggro code (once aggro is taken, enemy will permanently be aggro to player no matter how far they are)
        if(distanceFromPlayer < aggroRadius) {
            aggroTaken = true;
            aggroRadius = 900;
        }

        
        //if player is within aggro distance follow and outside of follow distance then follow (stops following if within follow radius)
        if(distanceFromPlayer < aggroRadius && distanceFromPlayer > followRadius && !isAttacking && !isKnocked) {
            rb.velocity = Vector3.zero;
            transform.position = Vector2.MoveTowards(this.transform.position, player.position, moveSpeed * Time.deltaTime);
        } else if(distanceFromPlayer <= followRadius && !isAttacking && !isKnocked) {
            rb.velocity = Vector2.up * moveSpeed * (Time.deltaTime + 1);
        }


        //Attk cooldown code
        if(distanceFromPlayer < aggroRadius) {
            if(!isAttacking) {
            attkTimer += Time.deltaTime;
            }

            if(attkTimer >= attkCooldown) {
                isAttacking = true;
                anim.SetBool("isAttking", true);
                transform.Rotate(new Vector3(0, 0, 20));
                attkTimer = 0;
                Attack();
            }
        }


        //attk stop code
        if(isAttacking) {
            if(distanceFromPlayer > followRadius + 10 || transform.GetChild(0).GetComponent<IgnorePlayerCol>().touching) {
                rb.velocity = Vector3.zero;
                isAttacking = false;
                anim.SetBool("isAttking", false);
                transform.Rotate(new Vector3(0, 0, -20));
            }
        }
        
    }


    void Flip() {
        isFacingRight = !isFacingRight;
        transform.Rotate(new Vector3(0, 180, 0));
    }


    void OnTriggerEnter2D(Collider2D col) {
        if(col.gameObject.layer == 8) {
            rb.velocity = Vector3.zero;
            isAttacking = false;
            anim.SetBool("isAttking", false);
            transform.Rotate(new Vector3(0, 0, -20));
        }
        if(col.gameObject.CompareTag("Player") && !playerHit) {
            col.gameObject.GetComponent<PlayerHealth>().TakeDamage(enemyDmg);
            playerHit = true;
        }
    }


    void OnTriggerExit2D(Collider2D col) {
        if(playerHit) {
            playerHit = false;
        }
    }


    void Attack() {
        Vector3 attkDir = (player.position - transform.position).normalized;
        rb.velocity = attkDir * attkingSpeed * (Time.deltaTime + 1);
    }


    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, aggroRadius);
        Gizmos.DrawWireSphere(transform.position, followRadius);
    }
}
