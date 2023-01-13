using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokTokController : MonoBehaviour
{
    //Variables for movement
    Rigidbody2D rb;
    [SerializeField] float moveSpeed;

    //variables for ai
    bool grounded;
    bool isFacingRight;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] GameObject groundCheck;
    [SerializeField] float circleRadius;
    [SerializeField] int enemyKnockback;

    //variables for attack
    [SerializeField] int enemyDmg;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isFacingRight = false;
    }

    // Update is called once per frame
    void Update()
    {
        //movement
        rb.velocity = -Vector2.right * moveSpeed * Time.deltaTime * 100;

        //ai (flipping at edges)
        grounded = Physics2D.OverlapCircle(groundCheck.transform.position, circleRadius, groundLayer);
        if(!grounded && isFacingRight) {
            Flip();
        } else if(!grounded && !isFacingRight) {
            Flip();
        }
    }

    void Flip() {
        isFacingRight = !isFacingRight;
        transform.Rotate(new Vector3(0, 180, 0));
        moveSpeed = -moveSpeed;
    }

    void OnCollisionEnter2D(Collision2D col) {
        if(col.gameObject.CompareTag("Player")) {
            Debug.Log("hit");
            col.gameObject.GetComponent<Health>().TakeDamage(enemyDmg);
            //col.gameObject.GetComponent<Rigidbody2D>().AddForce(col.gameObject.transform.right * enemyKnockback, ForceMode2D.Impulse);
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.transform.position, circleRadius);
    }
}
