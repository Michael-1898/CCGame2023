using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    // float deathTimer;
    // [SerializeField] float deathTime;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 2);
    }

    // Update is called once per frame
    void Update()
    {
        // deathTimer += Time.deltaTime;
        // if(deathTimer >= deathTime) {
        //     Destroy(gameObject);
        // }
    }
}
