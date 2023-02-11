using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToucanAttk : MonoBehaviour
{
    //attk variables
    [SerializeField] float attkCooldown;
    float attkTimer;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if attkTimer has reached attk cooldown, randomize an object, activate its sprite, and drop it shortly after actually instantiating the object
        //(alternatively just instantiate object once cool down has reached, but have it not fall until a bit after)
        //if attk type/num is the anvil do special dash

        //have seperate script for cocona as it doesn't just fall
    }

    void Attack() {

    }
}
