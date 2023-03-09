using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb2_Script : MonoBehaviour
{
    [SerializeField] float explosionTime;
    float explosionTimer;
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
        explosionTimer += Time.deltaTime;
        if(explosionTimer >= explosionTime) {
            Collider2D hitObject = Physics2D.OverlapCircle(transform.position, explosionRadius, playerLayer);
            if(hitObject != null && !hitObject.Equals(null)) {
                hitObject.GetComponent<PlayerHealth>().TakeDamage(bombDmg);
            }
            Destroy(gameObject);
        }
    }
}
