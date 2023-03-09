using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blop : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rb;
    bool grounded = true;
    bool jump = false;
    bool seePlayer = false;
    int direction = 1;
    float speed = 0;
    float rageTimer = 0f;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //checks for player
        jump = false;

        //checks if grounded
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(-0.5f * direction, -1.01f, 0f), Vector2.up * -1, .05f);
        if (hit.collider != null)
        {
            grounded = true;
        }
        else grounded = false;

        print(grounded);
        hit = Physics2D.Raycast(transform.position + new Vector3(0.6f * direction, 0.5f, 0f), Vector2.right * direction, 10f);

        if (hit.collider != null)
        {
            if(hit.collider.gameObject.name == "Player" || hit.collider.gameObject.name == "Player(Clone)")
            {
                jump = true;
                seePlayer = true;
                rageTimer = 1.5f;
            }
            else
            {
                seePlayer = false;
            }
        }
        
        //checks for hole in ground
        hit = Physics2D.Raycast(transform.position + new Vector3(1.2f*direction, -1f, 0f), Vector2.up*-1, 3f);
        if(hit.collider == null)
        {
            if(grounded)
            {
                print("change direction");
                direction *= -1;
            }
        }
        

        if (rageTimer > 0f)
        {
            hit = Physics2D.Raycast(transform.position + new Vector3(0.6f * direction, -0.5f, 0f), Vector2.right * direction, 3f);
            jump = true;
        }
        else
        {
            hit = Physics2D.Raycast(transform.position + new Vector3(0.6f * direction, -0.5f, 0f), Vector2.right * direction, 2f);
            jump = true;
        }
            
        if (hit.collider != null && jump == true && (hit.collider.gameObject.name != "Player" && hit.collider.gameObject.name != "Player(Clone)"))
        {
            hit = Physics2D.Raycast(transform.position + new Vector3(0.6f*direction, 0.5f, 0f), Vector2.right*direction, 5.0f);
            if(hit.collider != null)
            {
                
                if(hit.collider.gameObject.name != "Player" && hit.collider.gameObject.name != "Player(Clone)")
                {
                    jump = false;
                    hit = Physics2D.Raycast(transform.position + new Vector3(0.6f*direction, .5f, 0f), Vector2.right*direction, 0.5f);
                    if(hit.collider != null)
                    {
                        direction *= -1;
                    } 
                }

                hit = Physics2D.Raycast(transform.position + new Vector3(0.6f * direction, -0.5f, 0f), Vector2.right * direction, 5.0f);
                if (hit.collider != null)
                {
                    if (hit.collider.gameObject.name != "Player" && hit.collider.gameObject.name != "Player(Clone)")
                    {
                        jump = false;
                        hit = Physics2D.Raycast(transform.position + new Vector3(0.6f * direction, -0.5f, 0f), Vector2.right * direction, 0.5f);
                        if (hit.collider != null)
                        {
                            direction *= -1;
                        }
                    }
                }

            }
            hit = Physics2D.Raycast(transform.position + new Vector3(0f, 1.01f, 0f), Vector2.up, 1.1f);
            if(hit.collider != null)
            {
                print(hit.collider.gameObject.name);
                jump = false;
                hit = Physics2D.Raycast(transform.position + new Vector3(0.6f * direction, -.5f, 0f), Vector2.right * direction, 0.5f);
                if (hit.collider != null && (hit.collider.gameObject.name != "Player" || hit.collider.gameObject.name != "Player(Clone)"))
                {
                    direction *= -1;
                }
            }
            if (grounded && jump && (direction/rb.velocity.x) > 0)    
            {
                rb.velocity = new Vector2(rb.velocity.x, 4.6f);
            }
        }  

        
        if(rageTimer > 0f)
        {
            rageTimer -= Time.deltaTime;
            speed = .2f;
        }
        else
        {
            speed = 0.08f;
        }
        if((direction == 1 && rb.velocity.x < 0) || (direction == -1 && rb.velocity.x > 0))
        {
            speed = 0.5f;
        }

        if(rb.velocity.x == 0)
        {
            direction *= -1;
        }
        
        rb.velocity += new Vector2(speed * direction, 0f);
        if (Mathf.Abs(rb.velocity.x) > 4f)
        {
            rb.velocity = new Vector3(rb.velocity.x* 0.98f, rb.velocity.y, 0f);
        }
    }
}
