using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToucanBoss : MonoBehaviour
{
    //fly variables
    [SerializeField] float minJumpCooldown;
    float jumpTimer;
    [SerializeField] float maxJumpCooldown;
    float jumpCooldown;
    // [SerializeField] float jumpChance;
    // [SerializeField] float jumpStrength;
    // bool jump;
    [SerializeField] float maxJumpStrength;
    [SerializeField] float minJumpStrength;
    float jumpStrength;
    Rigidbody2D rb;
    bool isRising;
    bool isDescending;

    //lateral movement variables
    [SerializeField] Transform player;
    bool isFacingRight;

    //variables for groundCheck
    bool isGrounded; //bool for ground check
    [SerializeField] LayerMask groundLayer;
    [SerializeField] GameObject groundCheck;
    [SerializeField] float circleRadius;

    //variables for animator
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpTimer = minJumpCooldown;
        anim = GetComponent<Animator>();
        jumpStrength = minJumpStrength;
        jumpCooldown = Random.Range(minJumpCooldown, maxJumpCooldown);
        isDescending = false;
        isRising = false;
    }

    // Update is called once per frame
    void Update()
    {
        //checks if player is touching ground using overlap circle
        isGrounded = Physics2D.OverlapCircle(groundCheck.transform.position, circleRadius, groundLayer);


        //if toucan gets too high or far below player increase or decrease jumpCooldown
        if(transform.position.y - player.position.y <= -5 && !isRising) {
            jumpCooldown /= 2;
            isRising = true;
        } else if(isRising) {
            isRising = false;
        }
        if(transform.position.y - player.position.y >= 9 && !isDescending) {
            jumpCooldown *= 1.02f;
            isDescending = true;
        } else if(isDescending) {
            isDescending = false;
        }
        Debug.Log("isRising: " + isRising);
        Debug.Log("isDescending: " + isDescending);


        //jump code animator
        if(anim.GetBool("isJumping") && rb.velocity.y < 0) {
            anim.SetBool("isJumping", false);
        }
        //jump cooldown and input code
        jumpTimer += Time.deltaTime;
        if(jumpTimer >= jumpCooldown) {
            Jump();
        } else if(isGrounded) {
            Jump();
        }


        //flipping to look at player
        if(transform.position.x < player.position.x && !isFacingRight) {
            Flip();
        } else if(transform.position.x > player.position.x && isFacingRight) {
            Flip();
        }
    }

    void Jump() {
        //float tempJumpStrength;
        // float rand = Random.value;
        // if(rand <= jumpChance) {
        //     jump = true;
        // }

        // if(jumpTimer >= maxJumpCooldown) {
        //     jump = true;
        // }

        // if(jump) {
        //     rb.velocity = Vector3.zero;

        //     // if((jumpTimer - minJumpCooldown) <= (maxJumpCooldown - jumpTimer)) {
        //     //     tempJumpStrength = jumpStrength + 0.5f;
        //     // } else {
        //     //     tempJumpStrength = jumpStrength - 1;
        //     // }

        //     rb.AddForce(transform.up * jumpStrength, ForceMode2D.Impulse);
        //     anim.SetBool("isJumping", true);
        //     jumpTimer = 0;
        // }

    

        //if jumpCooldown is low do a small jump if high do a little 
        if(!isGrounded) {
            float jumpCooldownDiff = maxJumpCooldown - minJumpCooldown;
            float jumpStrengthDiff = maxJumpStrength - minJumpStrength;
            if(jumpCooldown <= (minJumpCooldown + (jumpCooldownDiff/3))) {
                jumpStrength = Random.Range(minJumpStrength, minJumpStrength + (jumpStrengthDiff/3));
            } else if(jumpCooldown <= (minJumpCooldown + (jumpCooldownDiff * (2/3)))) {
                jumpStrength = Random.Range(minJumpStrength + (jumpStrengthDiff/3), minJumpStrength + (jumpStrengthDiff * (2/3)));
            } else if(jumpCooldown <= maxJumpCooldown) {
                jumpStrength = Random.Range(minJumpStrength + (jumpStrengthDiff * (2/3)), maxJumpStrength);
            }

            //actual jump movement code
            rb.velocity = Vector3.zero;
            rb.AddForce(transform.up * jumpStrength, ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);
            jumpTimer = 0;
        } else if(isGrounded) {
            rb.velocity = Vector3.zero;
            rb.AddForce(transform.up * jumpStrength, ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);
            jumpTimer = 0;
        }
        //new jump cooldown
        jumpCooldown = Random.Range(minJumpCooldown, maxJumpCooldown);
        
    }

    void Flip() {
        isFacingRight = !isFacingRight;
        transform.Rotate(new Vector3(0, 180, 0));
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.transform.position, circleRadius);
    }
}
