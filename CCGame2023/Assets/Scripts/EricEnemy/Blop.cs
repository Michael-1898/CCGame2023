using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blop : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rb;
    bool jump = false;
    bool seePlayer = false;
    int direction = 1;
    float speed = 2;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //checks for player
        jump = false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0.6f*direction, 0.1f, 0f), Vector2.right*direction, 10f);


        if(hit.collider != null)
        {
            if(hit.collider.gameObject.name == "Player")
            {
                speed = 0.15f;
                jump = true;
                seePlayer = true;
            }
            else
            {
                speed = 0;
                seePlayer = false;
                rb.velocity = new Vector2(4f * direction, rb.velocity.y);
            }
        }
        else
        {
            jump = true;
              
                
            speed = 0;
            rb.velocity = new Vector2(4f * direction, rb.velocity.y);
            
            //checks for hole in ground
            hit = Physics2D.Raycast(transform.position + new Vector3(1f*direction, -1f, 0f), Vector2.up*-1, 2f);
            if(hit.collider == null)
            {
                direction *= -1;
            }
        }

        if(seePlayer)
        {
            hit = Physics2D.Raycast(transform.position + new Vector3(0.6f*direction, -0.5f, 0f), Vector2.right*direction, 3f);
        }   
        else
        {
            hit = Physics2D.Raycast(transform.position + new Vector3(0.6f*direction, -0.5f, 0f), Vector2.right*direction, 2f);
        } 
        if(hit.collider != null && jump == true)
        {
            if(hit.collider.gameObject.name != "Player")    
            {
                rb.AddForce(Vector2.up * 15f);
            }    
        }  


        


        rb.velocity += new Vector2(speed*direction, 0f);
        if(Mathf.Abs(rb.velocity.x) > 4f)
        {
           rb.velocity = new Vector3(rb.velocity.x* 0.99f, rb.velocity.y, 0f);
        }

    }
}
