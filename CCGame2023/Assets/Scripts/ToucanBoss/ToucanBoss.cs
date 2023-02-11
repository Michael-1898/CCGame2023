using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToucanBoss : MonoBehaviour
{
    //fly variables
    [SerializeField] float maxJumpStrength;
    [SerializeField] float midJumpStrength;
    [SerializeField] float minJumpStrength;
    float jumpStrength;
    Rigidbody2D rb;
    [SerializeField] float upperFlightBound;
    [SerializeField] float midFlightBound;
    [SerializeField] float lowerFlightBound;
    float flightBound;
    bool flightBoundSet;

    //lateral movement variables
    [SerializeField] Transform player;
    bool isFacingRight;
    [SerializeField] float leftBound;
    [SerializeField] float rightBound;
    [SerializeField] float moveSpeed;
    Vector2 reference;

    //variables for groundCheck
    bool isGrounded; //bool for ground check
    [SerializeField] LayerMask groundLayer;
    [SerializeField] GameObject groundCheck;
    [SerializeField] float circleRadius;

    //knockbacked variables
    float hitTimer;
    [SerializeField] float stunTime;
    bool isKnocked;

    //dmg variables
    bool playerHit;
    [SerializeField] int enemyDmg;
    [SerializeField] float enemyKnockback;

    //variables for animator
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        jumpStrength = minJumpStrength;
        flightBoundSet = false;
        isFacingRight = false;
        reference = player.position;
    }

    // Update is called once per frame
    void Update()
    {
        //checks if player is touching ground using overlap circle
        isGrounded = Physics2D.OverlapCircle(groundCheck.transform.position, circleRadius, groundLayer);
    

        if(GetComponent<Health>().hit == true) {
            isKnocked = true;
        }
        if(GetComponent<Health>().hit == true && isKnocked == true) {
            hitTimer = 0;
        }
        if(isKnocked) {
            hitTimer += Time.deltaTime;
        }
        if(hitTimer >= stunTime && isKnocked) {
            rb.velocity = Vector3.zero;
            hitTimer = 0;
            isKnocked = false;
        }

        
        //lateral movement
        if(!isKnocked) {
            Vector2 velocity = rb.velocity;
            
            if(transform.position.x - player.position.x <= leftBound) { //if too far left
                //move right
                velocity.x = moveSpeed * (Time.deltaTime + 1);
                if(!isFacingRight) {
                    Flip();
                    reference = player.position;
                }
            } else if(transform.position.x - player.position.x >= rightBound) { //if too far right
                //move left
                velocity.x = -moveSpeed * (Time.deltaTime + 1);
                if(isFacingRight) {
                    Flip();
                    reference = player.position;
                }
            } else if(velocity.x == 0) {
                if(transform.position.x < player.position.x) {
                    velocity.x = moveSpeed * (Time.deltaTime + 1);
                    if(!isFacingRight) {
                        Flip();
                        reference = player.position;
                    }
                } else if(transform.position.x > player.position.x) {
                    velocity.x = -moveSpeed * (Time.deltaTime + 1);
                    if(isFacingRight) {
                        Flip();
                        reference = player.position;
                    }
                }
                
            }
            rb.velocity = velocity;
            
        }
        


        //jump code animator
        if(anim.GetBool("isJumping") && rb.velocity.y < 0) {
            anim.SetBool("isJumping", false);

            //set new flight bound
            flightBoundSet = false;
        }


        if(!flightBoundSet) {
            int rand = Random.Range(1,4);
            if(rand == 1) {
                flightBound = upperFlightBound;
            } else if(rand == 2) {
                flightBound = midFlightBound;
            } else if(rand == 3) {
                flightBound = lowerFlightBound;
            }
            flightBoundSet = true;
        }


        //jump input code
        if(!isKnocked && transform.position.y - reference.y <= flightBound || isGrounded) {
            Jump();
        }
    }

    void Jump() {
        //setting jump strength according to flight bound and if it hit the ground 
        if(!isGrounded) {
            if(flightBound == upperFlightBound) {
                jumpStrength = minJumpStrength;
            } else if(flightBound == midFlightBound) {
                jumpStrength = midJumpStrength;
            } else if(flightBound == lowerFlightBound) {
                jumpStrength = maxJumpStrength;
            }
        } else if(isGrounded) {
            jumpStrength = midJumpStrength;
        }

        //actual code making the toucan jump
        Vector2 velocity = rb.velocity;
        rb.velocity = new Vector2(velocity.x, 0);
        rb.AddForce(transform.up * jumpStrength, ForceMode2D.Impulse);
        anim.SetBool("isJumping", true);
    }

    void Flip() {
        isFacingRight = !isFacingRight;
        transform.Rotate(new Vector3(0, 180, 0));
    }

    void OnTriggerEnter2D(Collider2D col) {
        if(col.gameObject.CompareTag("Player") && !playerHit) {
            col.gameObject.GetComponent<PlayerHealth>().TakeDamage(enemyDmg);
            playerHit = true;

            //set kb time for player
            col.gameObject.GetComponent<MJB_PlayerMove>().kbCurrentTime = col.gameObject.GetComponent<MJB_PlayerMove>().kbTotalTime;

            if(transform.position.x < col.transform.position.x) {   //if player is on right
                col.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.right * enemyKnockback * (Time.deltaTime + 1);
            } else {    //if player is on left
                col.gameObject.GetComponent<Rigidbody2D>().velocity = -Vector2.right * enemyKnockback * (Time.deltaTime + 1);
            }
        }
    }

    void OnTriggerExit2D(Collider2D col) {
        if(playerHit) {
            playerHit = false;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.transform.position, circleRadius);

        Gizmos.DrawLine(new Vector2(player.position.x - 20, player.position.y + upperFlightBound), new Vector2(player.position.x + 20, player.position.y + upperFlightBound));
        Gizmos.DrawLine(new Vector2(player.position.x - 20, player.position.y + midFlightBound), new Vector2(player.position.x + 20, player.position.y + midFlightBound));
        Gizmos.DrawLine(new Vector2(player.position.x - 20, player.position.y + lowerFlightBound), new Vector2(player.position.x + 20, player.position.y + lowerFlightBound));

        Gizmos.DrawLine(new Vector2(player.position.x + leftBound, player.position.y - 10), new Vector2(player.position.x + leftBound, player.position.y + 10));
        Gizmos.DrawLine(new Vector2(player.position.x + rightBound, player.position.y - 10), new Vector2(player.position.x + rightBound, player.position.y + 10));
    }
}
