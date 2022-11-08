using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MJB_PlayerMove : MonoBehaviour
{
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

    public Animator myAnim;
    public bool isWalking;
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
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = transform.position;

        Vector2 velocity = rb.velocity;
        velocity.x = Input.GetAxis("Horizontal") * playerSpeed * (Time.deltaTime + 1);
        rb.velocity = velocity;

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

        //sets walking bool for animation
        if(Input.GetAxis("Horizontal") != 0 && !isWalking) {
            isWalking = true;
        }
        if(Input.GetAxis("Horizontal") <= 0.9 && Input.GetAxis("Horizontal") >= -0.9 && isWalking == true) {
            isWalking = false;
        }        

        RaycastHit2D ground = Physics2D.BoxCast(new Vector2(playerPos.x, playerPos.y -0.56f), new Vector2(0.95f, 0.1f), 0, -Vector2.up, 0.1f); //checking if player is touching ground using boxcast

        if(ground.collider != null)
        {
            isGrounded = true;
            numJumps = maxJumps;
        } else {
            isGrounded = false;
            if(numJumps == maxJumps)
            {
                numJumps--;
            }
        }
        
        if((Input.GetKeyDown("up")) && (isGrounded == true || numJumps > 0))
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            numJumps--;

            velocity.y = 1 * jumpForce;
            rb.velocity = velocity;

            if(rb.gravityScale == 0) {
                rb.gravityScale = 5;
            }
        }

        if((Input.GetKey("up")) && isJumping == true)
        {
            if(jumpTimeCounter > 0)
            {
                velocity.y = 1 * jumpForce;
                rb.velocity = velocity;

                jumpTimeCounter -= Time.deltaTime;
            } else {
                isJumping = false;
            }
        }


        if(Input.GetKeyUp("up"))
        {
            isJumping = false;
        }
    }
}
