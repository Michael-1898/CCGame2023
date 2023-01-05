using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokTokController : MonoBehaviour
{
    //Variables for movement
    Rigidbody2D rb;
    Vector3 playerPos;
    [SerializeField] private float moveSpeed;

    private bool isAlive;
    private bool isGrounded;
    private bool isFacingRight;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isAlive = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
