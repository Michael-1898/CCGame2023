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
    bool isGrounded;
    private int numJumps;
    [SerializeField] private int maxJumps;
    public float playerRotation;

    //variables for attacking
    // public int attkNum;
    public float attkTimer;
    // public float comboTime;
    public bool isAttacking;

    //variables for animator
    public Animator myAnim;
    public static MJB_PlayerMove instance;

    private void Awake() {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        isJumping = false;
        rb = GetComponent<Rigidbody2D>();
        playerRotation = 0;
        //attkNum = 0;
        isAttacking = false;
        myAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //player movement
        playerPos = transform.position;
        Vector2 velocity = rb.velocity;
        velocity.x = Input.GetAxis("Horizontal") * playerSpeed * (Time.deltaTime + 1);
        rb.velocity = velocity;

        //sets playerSpeed parameter for animator
        myAnim.SetFloat("PlayerSpeed", Mathf.Abs(velocity.x));


        //flips character to face correct direction when walking
        if(Input.GetAxis("Horizontal") < 0)
        {
            playerRotation = 0;    
        }
        if(Input.GetAxis("Horizontal") > 0)
        {
            playerRotation = 180;    
        }
        transform.rotation = Quaternion.Euler(0f, playerRotation, 0f);


        //attack code (animator)
        if(Input.GetKeyDown("z") && !isAttacking) {
            isAttacking = true;
        }


        //checks if player is touching ground using boxcast: BoxCast(origin, size, direction, distance)
        RaycastHit2D ground = Physics2D.BoxCast(new Vector2(playerPos.x, playerPos.y -0.1f), new Vector2(0.2f, 0.02f), 0, -Vector2.up, 0.1f);

        //grounded code (uses boxcast to determine if player is grounded)
        if((ground.collider != null) && (ground.collider.gameObject != this.gameObject))
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
        if((Input.GetKeyDown("up")) && (isGrounded == true || numJumps > 0))
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
        if((Input.GetKey("up")) && isJumping == true)
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
        if(Input.GetKeyUp("up"))
        {
            isJumping = false;
            myAnim.SetBool("isJumping", false);
            myAnim.SetBool("JumpPeaked", true);
        }
    }

    //attack code
    void Attack() {

    }
}
