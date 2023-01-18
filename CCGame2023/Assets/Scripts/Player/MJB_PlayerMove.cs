using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //variables for attk func
    [SerializeField] private float attkLunge;
    public int attkNum;
    public bool attkStart;
    [SerializeField] GameObject attkPoint1;
    [SerializeField] GameObject attkPoint2;
    [SerializeField] float attkRadius;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] int playerDamage;
    [SerializeField] int playerKnockback;
    Vector2 attkPos;

    //variables for knockback
    public float kbTotalTime;
    public float kbCurrentTime;

    //variables for groundCheck
    bool ground;
    bool isGrounded;
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
    }

    // Update is called once per frame
    void Update()
    {
        //checks if player is touching ground using overlap circle
        ground = Physics2D.OverlapCircle(groundCheck.transform.position, circleRadius, groundLayer);


        //player movement
        playerPos = transform.position;
        Vector2 velocity = rb.velocity;
        if(kbCurrentTime <= 0) {
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
        if(Input.GetKeyDown("x") && !isAttacking && !comboDone && (ground)) {
            isAttacking = true;
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
                
                GetComponent<PlayerHealth>().invincible = false;
            }
        }

        if(attkStart) {
            Attack();
        }


        //grounded code (uses boxcast to determine if player is grounded)
        if(ground)
        {
            isGrounded = true;
            numJumps = maxJumps;

            myAnim.SetBool("isJumping", false);
            myAnim.SetBool("JumpPeaked", false);
        } else {
            isGrounded = false;
            
            if(numJumps == maxJumps)
            {
                numJumps--;
            }
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
        //sets attk point based on which attk num (attk 2 and 3 have larger and further out hitboxes)
        if(attkNum == 1) {
            attkPos = attkPoint1.transform.position;
        }
        //attack lunges
        if(attkNum == 2) {
            rb.AddForce(-transform.right * attkLunge, ForceMode2D.Impulse);
            attkPos = attkPoint2.transform.position;
            attkRadius *= 1.2f;
        }
        if(attkNum == 3) {
            rb.AddForce(-transform.right * 1.4f * attkLunge, ForceMode2D.Impulse);
            attkPos = attkPoint2.transform.position;

            //adds invincibility on third attack
            GetComponent<PlayerHealth>().invincible = true;
        }

        //if attack hits something do damage and knockback
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attkPos, attkRadius, enemyLayer);

        //foreach collider in hit enemies do damage/knock back
        foreach(Collider2D hitEnemy in hitEnemies) {

            //get component to get health/damage script of enemy and apply damage 
            hitEnemy.GetComponent<Health>().TakeDamage(playerDamage);

            //knockback
            if(attkNum < 3) {
                //knockback according to direction (if hit enemy is on left or right of player)
                if(transform.position.x < hitEnemy.transform.position.x) {
                    hitEnemy.GetComponent<Rigidbody2D>().velocity = Vector2.right * playerKnockback * (Time.deltaTime + 1);
                } else {
                    hitEnemy.GetComponent<Rigidbody2D>().velocity = -Vector2.right * playerKnockback * (Time.deltaTime + 1);
                }
            } else if(attkNum == 3) {
                if(transform.position.x < hitEnemy.transform.position.x) {
                    hitEnemy.GetComponent<Rigidbody2D>().velocity = Vector2.right * (playerKnockback * 3.5f) * (Time.deltaTime + 1);
                } else {
                    hitEnemy.GetComponent<Rigidbody2D>().velocity = -Vector2.right * (playerKnockback * 3.5f) * (Time.deltaTime + 1);
                }
            }
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.transform.position, circleRadius);
        Gizmos.DrawWireSphere(attkPoint1.transform.position, attkRadius);
        Gizmos.DrawWireSphere(attkPos, attkRadius);
    }
}
