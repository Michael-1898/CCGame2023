using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb1_Script : MonoBehaviour
{
    [SerializeField] int bombDmg;
    [SerializeField] float explosionRadius;
    [SerializeField] LayerMask playerLayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D col) {
        Collider2D hitObject = Physics2D.OverlapCircle(transform.position, explosionRadius, playerLayer);
        if(hitObject) {
            hitObject.GetComponent<PlayerHealth>().TakeDamage(bombDmg);
        }
        Destroy(gameObject);
    }
    
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
