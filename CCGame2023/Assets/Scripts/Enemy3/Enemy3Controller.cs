using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3Controller : MonoBehaviour
{
    //movement variables
    [SerializeField] float aggroRadius;
    [SerializeField] float followRadius;
    [SerializeField] float runRadius;
    [SerializeField] float moveSpeed;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] Transform player;
    bool aggroTaken;

    //attk variables
    [SerializeField] int enemyDmg;
    [SerializeField] int enemyKnockback;
    [SerializeField] float attkCooldown;
    float attkTimer;
    [SerializeField] GameObject bullet;

    //knockbacked variables
    float hitTimer;
    [SerializeField] float stunTime;
    bool isKnocked;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        aggroTaken = false;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
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
        }


        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);

        //initial aggro code (once aggro is taken, enemy will permanently be aggro to player no matter how far they are)
        if(distanceFromPlayer < aggroRadius) {
            aggroTaken = true;
            aggroRadius = 900;
        }


        //if player is within aggro distance follow and outside of follow distance then follow (stops following if within follow radius)
        if(distanceFromPlayer < aggroRadius && distanceFromPlayer > followRadius && !isKnocked) {
            transform.position = Vector2.MoveTowards(this.transform.position, player.position, moveSpeed * Time.deltaTime);
            rb.velocity = Vector3.zero;
        } else if(distanceFromPlayer <= followRadius && distanceFromPlayer > runRadius && !isKnocked) {
            if(transform.position.y <= player.position.y + 1.5) {
                rb.velocity = Vector2.up * (moveSpeed/2) * (Time.deltaTime + 1);
            } else {
                rb.velocity = Vector3.zero;   
            }
        } else if(distanceFromPlayer <= runRadius && !isKnocked) {
            transform.position = Vector2.MoveTowards(this.transform.position, player.position, -moveSpeed * Time.deltaTime);
            rb.velocity = Vector3.zero;
        }


        //Attk cooldown code
        if(distanceFromPlayer < aggroRadius) {
            attkTimer += Time.deltaTime;

            if(attkTimer >= attkCooldown) {
                attkTimer = 0;
                Attack();
            }
        }
    }


    void OnCollisionEnter2D(Collision2D col) {
        rb.velocity = Vector3.zero;
        
        if(col.gameObject.CompareTag("Player")) {   //if collided with player
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


    void Attack() {
        Instantiate(bullet, transform.position, Quaternion.identity);
    }


    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, aggroRadius);
        Gizmos.DrawWireSphere(transform.position, followRadius);
        Gizmos.DrawWireSphere(transform.position, runRadius);
    }
}
