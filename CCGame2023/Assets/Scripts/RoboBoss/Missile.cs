using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    GameObject player;

    //missile behavior variables
    [SerializeField] float launchTime;
    float launchTimer;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        launchTimer += Time.deltaTime;
        if(launchTimer >= launchTime) {
            //stop moving up
            rb.velocity = Vector3.zero;

            //orientate towards player

            //launch at player
        }
    }
}
