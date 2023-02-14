using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cocona : MonoBehaviour
{
    GameObject Player;
    Vector3 playerPos;
    Vector3 dir;

    //bullet variables
    public float Speed;
    [SerializeField] float liftTime;
    float timer;

    Rigidbody2D rb;

    //collision variables
    [SerializeField] int dmg;
    [SerializeField] float knockback;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        playerPos = Player.transform.position;

        timer = 0;

        rb = GetComponent<Rigidbody2D>();

        rb.AddForce(Vector2.up * 150 * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        if(timer < liftTime) {
            playerPos = Player.transform.position;
            Vector2 velocity = (playerPos - transform.position).normalized * Speed * (Time.deltaTime + 1);
            rb.velocity = new Vector2(velocity.x, rb.velocity.y);
            dir = (playerPos - transform.position);
        } else {
            rb.velocity = dir.normalized * Speed * (Time.deltaTime + 1);
        }

        
        timer += Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D col) {
        if(col.gameObject.CompareTag("Player")) {   //if collided with player
            //deal damage
            col.gameObject.GetComponent<PlayerHealth>().TakeDamage(dmg);
            Destroy(this.gameObject);
        }

        Destroy(this.gameObject, 0.3f);
    }
}
