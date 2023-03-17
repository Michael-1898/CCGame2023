using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoboMovement : MonoBehaviour
{
    //Variables for movement
    Rigidbody2D rb;
    [SerializeField] float moveSpeed;

    [SerializeField] float minMoveMult; //min movement multiplier to speed up/slow down movemet
    [SerializeField] float maxMoveMult; //exclusive
    float moveMult;

    [SerializeField] float chaseRange; //if player leaves this radius, the boss will start chasing the player
    Transform player;
    bool isChasing;
    
    float moveTimer;
    [SerializeField] float moveCooldown;
    float timeMoving; //time spent moving
    bool isMoving;

    float movementDuration; //how long it will be moving for
    [SerializeField] float minMoveDur;
    [SerializeField] float maxMoveDur;


    //variables for flipping
    [SerializeField] float circleRadius;
    [SerializeField] GameObject edgeCheckL;
    [SerializeField] GameObject edgeCheckR;
    [SerializeField] LayerMask groundLayer;
    bool edged;


    //variables for animation
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if(moveSpeed > 0) {
            moveSpeed *= -1;
        }
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);

        //if reaches edge move in other direction
        if(edged && Physics2D.OverlapCircle(edgeCheckL.transform.position, circleRadius, groundLayer) == false) {
            edged = false;
            if(moveSpeed < 0) {
                moveSpeed *= -1;
            }
            anim.SetBool("Direction", false);
            movementDuration = 0.5f;
            isMoving = true;

        } else if(edged && Physics2D.OverlapCircle(edgeCheckR.transform.position, circleRadius, groundLayer) == false) {
            edged = false;
            if(moveSpeed > 0) {
                moveSpeed *= -1;
            }
            anim.SetBool("Direction", true);
            movementDuration = 0.5f;
            isMoving = true;

        } else if(!edged && Physics2D.OverlapCircle(edgeCheckL.transform.position, circleRadius, groundLayer) == true && Physics2D.OverlapCircle(edgeCheckR.transform.position, circleRadius, groundLayer) == true) {
            edged = true;
        }

        moveTimer += Time.deltaTime;
        if(moveTimer >= moveCooldown && !isMoving) {
            int random = Random.Range(0,2); //randomizes move direciton (left or right)
            if(random == 0 && moveSpeed > 0) { //if movespeed is positive/right
                moveSpeed *= -1;
                anim.SetBool("Direction", true);
            } else if(random == 1 && moveSpeed < 0) {
                moveSpeed *= -1;
                anim.SetBool("Direction", false);
            }

            //randomizes movement multiplier and movement duration
            moveMult = Random.Range(minMoveMult, maxMoveMult);
            movementDuration = Random.Range(minMoveDur, maxMoveDur);

            isMoving = true;
            anim.SetBool("isMoving", true);
        }

        if(distanceFromPlayer > chaseRange && !isChasing) {
            if(player.position.x < transform.position.x && moveSpeed > 0) {
                moveSpeed *= -1;
            } else if(player.position.x > transform.position.x && moveSpeed < 0) {
                moveSpeed *= -1;
            }
            moveMult = maxMoveMult;
            isChasing = true;
            moveTimer = moveCooldown;
        } else if(distanceFromPlayer <= chaseRange && isChasing) {
            isChasing = false;
        }

        if(isMoving) {
            rb.velocity = Vector2.right * moveSpeed * moveMult * (Time.deltaTime + 1);
            
            if(!isChasing) {
                timeMoving += Time.deltaTime;
            }
            
            if(timeMoving >= movementDuration) {
                moveTimer = 0;
                anim.SetBool("isMoving", false);
                isMoving = false;
                timeMoving = 0;
            }
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(edgeCheckL.transform.position, circleRadius);
        Gizmos.DrawWireSphere(edgeCheckR.transform.position, circleRadius);
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}