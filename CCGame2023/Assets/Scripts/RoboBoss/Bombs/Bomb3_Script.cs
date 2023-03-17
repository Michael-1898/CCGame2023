using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb3_Script : MonoBehaviour
{
    [SerializeField] float lifeTime;
    float lifeTimer;
    [SerializeField] GameObject explosionFX;
    Transform player;
    [SerializeField] GameObject triggerCollider;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }

    // Update is called once per frame
    void Update()
    {
        lifeTimer += Time.deltaTime;
        if(lifeTimer >= lifeTime) {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D col) {
        
    }
}
