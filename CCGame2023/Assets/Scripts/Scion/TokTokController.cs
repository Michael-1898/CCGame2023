﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokTokController : MonoBehaviour
{
    //Variables for movement
    Rigidbody2D rb;
    [SerializeField] float moveSpeed;

    //variables for ai
    bool grounded;
    bool hardGrounded;
    bool walled;
    bool isFacingRight;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] GameObject groundCheck;
    [SerializeField] GameObject wallCheck;
    [SerializeField] float circleRadius;
    [SerializeField] int enemyKnockback;
    [SerializeField] GameObject hardGroundCheck;

    //variables for knockback
    [SerializeField] float kbTime;
    float kbTimer;

    //variables for attack
    [SerializeField] int enemyDmg;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isFacingRight = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Physics2D.OverlapCircle(hardGroundCheck.transform.position, circleRadius, groundLayer) == false && hardGrounded) {
            hardGrounded = false;
        } else if(Physics2D.OverlapCircle(groundCheck.transform.position, circleRadius, groundLayer) == true && !hardGrounded) {
            hardGrounded = true;
        }

        //movement
        if(gameObject.GetComponent<Health>().hit == true) { //if hit, start kb timer
            kbTimer = kbTime;
        }
        if(kbTimer <= 0 && hardGrounded) {  //if there is no knockback, do movement
            rb.velocity = -Vector2.right * moveSpeed * (Time.deltaTime + 1);
        } else {    //if there is knock back, subtract kb timer
            kbTimer -= Time.deltaTime;
        }


        //ai (flipping at edges)
        grounded = Physics2D.OverlapCircle(groundCheck.transform.position, circleRadius, groundLayer);
        walled = Physics2D.OverlapCircle(wallCheck.transform.position, circleRadius, groundLayer | enemyLayer);
        if((!grounded || walled) && isFacingRight && hardGrounded) {
            Flip();
        } else if((!grounded || walled) && !isFacingRight && hardGrounded) {
            Flip();
        }
    }


    void Flip() {
        isFacingRight = !isFacingRight;
        transform.Rotate(new Vector3(0, 180, 0));
        moveSpeed = -moveSpeed;
    }


    void OnCollisionEnter2D(Collision2D col) {
        if(col.gameObject.CompareTag("Player") && col.gameObject.GetComponent<MJB_PlayerMove>().kbCurrentTime <= 0) {   //if collided with player
            //deal damage
            col.gameObject.GetComponent<PlayerHealth>().TakeDamage(enemyDmg);

            //set kb time for player
            col.gameObject.GetComponent<MJB_PlayerMove>().kbCurrentTime = col.gameObject.GetComponent<MJB_PlayerMove>().kbTotalTime;

            if(transform.position.x < col.transform.position.x) {   //if player is on right
                col.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.right * enemyKnockback * (Time.deltaTime + 1);
            } else {    //if player is on left
                col.gameObject.GetComponent<Rigidbody2D>().velocity = -Vector2.right * enemyKnockback * (Time.deltaTime + 1);
            }
        }
    }


    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.transform.position, circleRadius);
        Gizmos.DrawWireSphere(hardGroundCheck.transform.position, circleRadius);
        Gizmos.DrawWireSphere(wallCheck.transform.position, circleRadius);
    }
}
