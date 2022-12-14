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
    private int numJumps;
    [SerializeField] private int maxJumps;
    private float playerRotation;

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
    [SerializeField] private GameObject attkPoint;
    [SerializeField] private Vector2 attkBoxSize;
    [SerializeField] private LayerMask enemyLayer;
    private GameObject enemy;

    //variables for groundCheck
    private bool ground;
    private bool isGrounded;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private GameObject groundCheck;
    [SerializeField] private float circleRadius;

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
        velocity.x = Input.GetAxis("Horizontal") * playerSpeed * (Time.deltaTime + 1);
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
                comboDone = false;
                attkTimer = 0f;
                attkNum = 0;
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
        //attack lunges
        if(attkNum == 2) {
            rb.AddForce(-transform.right * attkLunge, ForceMode2D.Impulse);
        }
        if(attkNum == 3) {
            rb.AddForce(-transform.right * 1.6f * attkLunge, ForceMode2D.Impulse);
        }

        //if attack hits something do damage and knockback
        if(Physics2D.OverlapBox(attkPoint.transform.position, attkBoxSize, enemyLayer)) {
            enemy = Physics2D.OverlapBox(attkPoint.transform.position, attkBoxSize, enemyLayer).gameObject;
            Debug.Log(enemy);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.transform.position, circleRadius);
        Gizmos.DrawWireCube(attkPoint.transform.position, attkBoxSize);
    }
}
