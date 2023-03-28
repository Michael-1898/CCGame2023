using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnorePlayerCol : MonoBehaviour
{
    public bool touching;
    // Start is called before the first frame update
    void Start()
    {
        touching = false;
        Physics2D.IgnoreCollision(GameObject.FindWithTag("Player").GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D col) {
        touching = true;
    }

    void OnCollisionExit2D(Collision2D col) {
        touching = false;
    }
}
