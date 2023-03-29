using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    GameObject player;
    //[SerializeField] private Camera cam;

    //missile behavior variables
    [SerializeField] float launchTime;
    float launchTimer;
    bool lockingOn;
    bool targetAcquired;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        launchTimer += Time.deltaTime;
        if(launchTimer >= launchTime) {
            //stop moving up
            rb.velocity = Vector3.zero;
            lockingOn = true;

            //orientate towards player

            //launch at player
        }

        if(lockingOn) {
            //point towards player
            Vector3 targetPosition = GameObject.FindWithTag("Player").transform.position;
            Vector3 dir = (targetPosition - transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
            transform.localRotation = Quaternion.Euler(0f, 0f, angle);
            
            targetAcquired = true;
            lockingOn = false;
        }

        if(targetAcquired) {
            //launch at player
        }
    }
}
