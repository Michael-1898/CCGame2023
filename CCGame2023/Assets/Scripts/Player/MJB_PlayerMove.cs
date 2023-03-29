﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MJB_PlayerMove : MonoBehaviour
{
    //variables for movement
    Rigidbody2D rb;
    public float jumpTime;
    private float jumpTimeCounter;
    public float playerSpeed;
    public float jumpForce;
    bool isJumping;
    Vector3 playerPos;
    int numJumps;
    [SerializeField] int maxJumps;
    float playerRotation;

    //variables for attacking
    public float attkTimer;
    public bool isAttacking;
    public bool comboHit;
    public bool comboDone;
    public float attkCooldown;
    public bool attkAnimPlaying;
    public bool canRotate;
    public int attkType; //int which uses numbers to describe type of attack (0 is normal, 1 is side aerial, 2 is down aerial, 3 is up aerial)
    
    float groundedTimer = 0f;
    Vector3 lastGroundedPosition;
    

    //variables for attk func
    [SerializeField] private float attkLunge;
    public int attkNum;
    public bool attkStart;
    [SerializeField] GameObject attkPoint1;
    [SerializeField] GameObject attkPoint2;
    [SerializeField] GameObject attkPointUp;
    [SerializeField] GameObject attkPointDown;
    [SerializeField] float attkRadius;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] int playerDamage;
    [SerializeField] int playerKnockback;
    [SerializeField] float attkPushback;
    Vector2 attkPos;

    //variables for knockback
    public float kbTotalTime;
    public float kbCurrentTime;

    //variables for groundCheck
    bool ground; //bool for ground check
    bool isGrounded; //bool used for jumping code
    [SerializeField] LayerMask groundLayer;
    [SerializeField] GameObject groundCheck;
    [SerializeField] float circleRadius;

    //variables for animator
    public Animator myAnim;
    public static MJB_PlayerMove instance;

    //way to make a var private but still access it from the animation scripts
    // public bool IsJumping
    // {
    //     get{ return isJumping;}
    //     set { isJumping = value;}
    // }

    private void Awake() {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        isJumping = false;
        rb = GetComponent<Rigidbody2D>();
        playerRotation = 0;
        isAttacking = false;
        myAnim = GetComponent<Animator>();
        comboDone = false;
        comboHit = false;
        attkTimer = 0f;
        attkAnimPlaying = false;
        canRotate = true;
        attkNum = 0;
        attkStart = false;
        lastGroundedPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //return to main menu
        if(Input.GetKeyDown("escape")) {
            SceneManager.LoadScene(0);
        }


        //checks if player is touching ground using overlap circle
        ground = Physics2D.OverlapCircle(groundCheck.transform.position, circleRadius, groundLayer);

        groundedTimer += Time.deltaTime;
        
        
        
        //player movement
        playerPos = transform.position;
        Vector2 velocity = rb.velocity;
        myAnim.SetFloat("yVelocity", velocity.y);
        if(kbCurrentTime <= 0 && canRotate) {
            velocity.x = Input.GetAxis("Horizontal") * playerSpeed * (Time.deltaTime + 1);
        } else {
            kbCurrentTime -= Time.deltaTime;
        }
        if(!attkAnimPlaying) {
            rb.velocity = velocity;
        }
        

        //sets playerSpeed parameter for animator
        myAnim.SetFloat("PlayerSpeed", Mathf.Abs(velocity.x));


        //flips character to face correct direction when walking
        if(canRotate) {
            if(Input.GetAxis("Horizontal") < 0)
            {
                //facing left
                playerRotation = 0;    
            }
            if(Input.GetAxis("Horizontal") > 0)
            {
                //facing right
                playerRotation = 180;    
            }
            transform.rotation = Quaternion.Euler(0f, playerRotation, 0f);
        }

       
        //attack code (animator)
        if(Input.GetKeyDown("x") && !isAttacking && !comboDone && ground && Input.GetKey("up")) {
            isAttacking = true;
            attkType = 2;
        } else if(Input.GetKeyDown("x") && !isAttacking && !comboDone && ground) {
            isAttacking = true;
            attkType = 0;
        } else if(Input.GetKeyDown("x") && !isAttacking && !comboDone && !ground && Input.GetKey("up")) {
            isAttacking = true;
            attkType = 2;
        } else if(Input.GetKeyDown("x") && !isAttacking && !comboDone && !ground && Input.GetKey("down")) {
            isAttacking = true;
            attkType = 3;
        } else if(Input.GetKeyDown("x") && !isAttacking && !comboDone && !ground) {
            isAttacking = true;
            attkType = 1;
        }

        if(comboDone == true) {
            attkTimer += Time.deltaTime;

            if(attkTimer >= attkCooldown) {
                if(attkNum > 1) {
                    attkRadius /= 1.2f;
                }

                comboDone = false;
                attkTimer = 0f;
                attkNum = 0;
                if(ground) {
                    rb.velocity = Vector3.zero;
                }
                
                GetComponent<PlayerHealth>().invincible = false;
            }
        }

        if(attkStart) {
            Attack();
        }


        //grounded code (uses overlap circle to determine if player is grounded)
        if(ground)
        {
            isGrounded = true;
            numJumps = maxJumps;
            myAnim.SetBool("isJumping", false);
            myAnim.SetBool("JumpPeaked", false);

            //code to save player location in case they fall off the map
            if (groundedTimer > 1f)
            {
                groundedTimer = 0f;
                lastGroundedPosition = transform.position;
            }

        } else {
            isGrounded = false;
            
            if(numJumps == maxJumps)
            {
                numJumps--;
            }
        }
        //print(GetComponent<PlayerHealth>().invincible);
        if (groundedTimer > 1f)
        {
            GetComponent<PlayerHealth>().invincible = false;
            if(isGrounded)
            {
                groundedTimer = 0f;
                lastGroundedPosition = transform.position;
            }
                
                
        }

        //code to check if player fell off map and then teleports them
        if (transform.position.y < -10f)
        {
            GetComponent<PlayerHealth>().TakeDamage(1);
            GetComponent<PlayerHealth>().invincible = true;
            groundedTimer = 0f;
            transform.position = lastGroundedPosition;
            numJumps = maxJumps;
            
            
        }

        //jump code (initial jump)
        if((Input.GetKeyDown("z")) && (isGrounded == true || numJumps > 0))
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            numJumps--;

            velocity.y = 1 * jumpForce;
            rb.velocity = velocity;

            myAnim.SetBool("isJumping", true);

            if(rb.gravityScale == 0) {
                rb.gravityScale = 5;
            }
        }

        //jump code (extended jump/holding down jump)
        if((Input.GetKey("z")) && isJumping == true)
        {
            if(jumpTimeCounter > 0)
            {
                velocity.y = 1 * jumpForce;
                rb.velocity = velocity;

                myAnim.SetBool("isJumping", true);

                jumpTimeCounter -= Time.deltaTime;
            } else {
                isJumping = false;
                myAnim.SetBool("isJumping", false);
                myAnim.SetBool("JumpPeaked", true);
            }
        }

        //jump code (ends jump when lifting button/taking finger off button)
        if(Input.GetKeyUp("z"))
        {
            isJumping = false;
            myAnim.SetBool("isJumping", false);
            myAnim.SetBool("JumpPeaked", true);
        }
    }

    //attack code
    void Attack() {
        //sets attk point based on which attk num (attk 2 and 3 have larger and further out hitboxes) & handles attk lunges
        if(attkNum == 1 && attkType < 2) {
            attkPos = attkPoint1.transform.position;
        } else if(attkNum == 2) {
            rb.velocity = Vector3.zero;
            rb.AddForce(-transform.right * attkLunge, ForceMode2D.Impulse);
            attkPos = attkPoint2.transform.position;
            attkRadius *= 1.2f;
        } else if(attkNum == 3) {
            rb.velocity = Vector3.zero;
            rb.AddForce(-transform.right * 1.2f * attkLunge, ForceMode2D.Impulse);
            attkPos = attkPoint2.transform.position;

            //adds invincibility on third attack
            GetComponent<PlayerHealth>().invincible = true;
        } else if(attkType == 2) {
            attkPos = attkPointUp.transform.position;
        } else if(attkType == 3) {
            attkPos = attkPointDown.transform.position;
        } else {
            attkPos = attkPoint1.transform.position;
        }

        //if attack hits something do damage and knockback
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attkPos, attkRadius, enemyLayer);

        //foreach collider in hit enemies do damage/knock back
        foreach(Collider2D hitEnemy in hitEnemies) {

            //get component to get health/damage script of enemy and apply damage
            if(!hitEnemy.gameObject.CompareTag("ObstacleCollider")) {
                hitEnemy.GetComponent<Health>().TakeDamage(playerDamage);

                //knockback
                if(attkNum < 3 && attkType < 2) {
                    //knockback according to direction (if hit enemy is on left or right of player)
                    if(transform.position.x < hitEnemy.transform.position.x) {
                        hitEnemy.GetComponent<Rigidbody2D>().velocity = Vector2.right * playerKnockback * (Time.deltaTime + 1);
                    } else {
                        hitEnemy.GetComponent<Rigidbody2D>().velocity = -Vector2.right * playerKnockback * (Time.deltaTime + 1);
                    }
                } else if(attkNum == 3 && attkType < 2 && !hitEnemy.gameObject.CompareTag("LargeBoss")) {
                    if(transform.position.x < hitEnemy.transform.position.x) {
                        hitEnemy.GetComponent<Rigidbody2D>().velocity = Vector2.right * (playerKnockback * 3.5f) * (Time.deltaTime + 1);
                    } else {
                        hitEnemy.GetComponent<Rigidbody2D>().velocity = -Vector2.right * (playerKnockback * 3.5f) * (Time.deltaTime + 1);
                    }
                } else if(attkNum == 3 && attkType < 2 && hitEnemy.gameObject.CompareTag("LargeBoss")) {
                    if(transform.position.x < hitEnemy.transform.position.x) {
                        hitEnemy.GetComponent<Rigidbody2D>().velocity = Vector2.right * (playerKnockback) * (Time.deltaTime + 1);
                    } else {
                        hitEnemy.GetComponent<Rigidbody2D>().velocity = -Vector2.right * (playerKnockback) * (Time.deltaTime + 1);
                    }
                } else if(attkType > 1) {
                    if(transform.position.y < hitEnemy.transform.position.y) {
                        hitEnemy.GetComponent<Rigidbody2D>().velocity = Vector2.up * playerKnockback * (Time.deltaTime + 1);
                    } else {
                        hitEnemy.GetComponent<Rigidbody2D>().velocity = -Vector2.up * playerKnockback * (Time.deltaTime + 1);
                    }
                }
            }
        }

        //knock player back after hitting
        if(hitEnemies.Length > 0) {
            if(attkType == 3) {
                rb.velocity = Vector2.up * attkPushback * (Time.deltaTime + 1);
                kbCurrentTime = kbTotalTime;
            }
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.transform.position, circleRadius);
        Gizmos.DrawWireSphere(attkPoint1.transform.position, attkRadius);
        Gizmos.DrawWireSphere(attkPos, attkRadius);
        Gizmos.DrawWireSphere(attkPointUp.transform.position, attkRadius);
        Gizmos.DrawWireSphere(attkPointDown.transform.position, attkRadius);
    }
}
