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
    
    float moveTimer;
    [SerializeField] float moveCooldown;
    float timeMoving; //time spent moving
    bool isMoving;

    float movementDuration; //how long it will be moving for
    [SerializeField] float minMoveDur;
    [SerializeField] float maxMoveDur;


    //variables for animation
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        moveTimer += Time.deltaTime;
        if(moveTimer >= moveCooldown && !isMoving) {
            int random = Random.Range(0,2); //randomizes move direciton (left or right)
            if(random == 0 && moveSpeed > 0) { //if movespeed is positive/right
                moveSpeed *= -1;
            } else if(random == 1 && moveSpeed < 0) {
                moveSpeed *= -1;
            }

            //randomizes movement multiplier and movement duration
            moveMult = Random.Range(minMoveMult, maxMoveMult);
            movementDuration = Random.Range(minMoveDur, maxMoveDur);

            isMoving = true;
            anim.SetBool("isMoving", true);
        }

        if(isMoving) {
            rb.velocity = Vector2.right * moveSpeed * moveMult * (Time.deltaTime + 1);
            
            timeMoving += Time.deltaTime;
            if(timeMoving >= movementDuration) {
                moveTimer = 0;
                anim.SetBool("isMoving", false);
                isMoving = false;
                timeMoving = 0;
            }
        }
    }
}