using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb3_Script : MonoBehaviour
{
    [SerializeField] float lifeTime;
    float lifeTimer;
    [SerializeField] int bombDmg;

    // Start is called before the first frame update
    void Start()
    {
        
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
        if(col.gameObject.CompareTag("Player")) {   //if collided with player
            //deal damage
            col.gameObject.GetComponent<PlayerHealth>().TakeDamage(bombDmg);
        }
    }
}
