﻿using System.Collections;
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



        rb.velocity = Vector3.zero;
        //if jumpCooldown is low do a small jump if high do a little jump
        rb.AddForce(transform.up * jumpStrength, ForceMode2D.Impulse);
        anim.SetBool("isJumping", true);
        jumpTimer = 0;
    }
}
