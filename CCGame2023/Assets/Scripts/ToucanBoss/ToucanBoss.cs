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

    //lateral movement variables
    [SerializeField] Transform player;
    bool isFacingRight;

    Rigidbody2D rb;

    //variables for animator
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpTimer = minJumpCooldown;
        anim = GetComponent<Animator>();
        jumpStrength = minJumpStrength;
    }

    // Update is called once per frame
    void Update()
    {
        //flipping to look at player
        if(transform.position.x < player.position.x && !isFacingRight) {
            Flip();
        } else if(transform.position.x > player.position.x && isFacingRight) {
            Flip();
        }


        if(anim.GetBool("isJumping") && rb.velocity.y < 0) {
            anim.SetBool("isJumping", false);
        }

        // if(jumpTimer == 0) {
        //     jump = false;
        // }
        

        jumpTimer += Time.deltaTime;
        jumpCooldown = Random.Range(minJumpCooldown, maxJumpCooldown);
        if(jumpTimer >= jumpCooldown) {
            Jump();
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
        float jumpCooldownDiff = maxJumpCooldown - minJumpCooldown;
        float jumpStrengthDiff = maxJumpStrength - minJumpStrength;
        if(jumpCooldown <= (minJumpCooldown + (jumpCooldownDiff/3))) {
            jumpStrength = Random.Range(minJumpStrength, minJumpStrength + (jumpStrengthDiff/3));
        } else if(jumpCooldown <= (minJumpCooldown + (jumpCooldownDiff * (2/3)))) {
            jumpStrength = Random.Range(minJumpStrength + (jumpStrengthDiff/3), minJumpStrength + (jumpStrengthDiff * (2/3)));
        } else if(jumpCooldown <= maxJumpCooldown) {
            jumpStrength = Random.Range(minJumpStrength + (jumpStrengthDiff * (2/3)), maxJumpStrength);
        }
        // Debug.Log(jumpCooldown);
        // Debug.Log(jumpStrength);

        rb.velocity = Vector3.zero;
        rb.AddForce(transform.up * jumpStrength, ForceMode2D.Impulse);
        anim.SetBool("isJumping", true);
        jumpTimer = 0;
    }

    void Flip() {
        isFacingRight = !isFacingRight;
        transform.Rotate(new Vector3(0, 180, 0));
    }
}
