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
        if(distanceFromPlayer < aggroRadius && distanceFromPlayer > followRadius) {
            transform.position = Vector2.MoveTowards(this.transform.position, player.position, moveSpeed * Time.deltaTime);
        } else if(distanceFromPlayer <= followRadius && distanceFromPlayer > runRadius) {
            transform.position = transform.position;
        } else if(distanceFromPlayer <= runRadius) {
            transform.position = Vector2.MoveTowards(this.transform.position, player.position, -moveSpeed * Time.deltaTime);
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


    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, aggroRadius);
        Gizmos.DrawWireSphere(transform.position, followRadius);
        Gizmos.DrawWireSphere(transform.position, runRadius);
    }
}
