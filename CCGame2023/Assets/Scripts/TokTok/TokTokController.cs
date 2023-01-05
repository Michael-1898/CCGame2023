using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokTokController : MonoBehaviour
{
    //Variables for movement
    Rigidbody2D rb;
    [SerializeField] private float moveSpeed;

    //variables for ai
    private bool grounded;
    private bool isFacingRight;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private GameObject groundCheck;
    [SerializeField] private float circleRadius;

    private bool isAlive;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isAlive = true;
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

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.transform.position, circleRadius);
    }
}
