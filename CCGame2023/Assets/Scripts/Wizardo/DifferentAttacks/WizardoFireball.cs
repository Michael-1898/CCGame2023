using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardoFireball : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject player;
    Rigidbody2D rb;
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        if(GameObject.Find("Player") != null)
        {
            player = GameObject.Find("Player");
        }
        else player = GameObject.Find("Player(Clone)");
        Vector3 direction = (player.transform.position + new Vector3(0f, 1f, 0f)) - transform.position;
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 10000);
    }

    // Update is called once per frame
    void Update()
    {   
        
        rb.AddForce(transform.up * 4f);
    }
}
