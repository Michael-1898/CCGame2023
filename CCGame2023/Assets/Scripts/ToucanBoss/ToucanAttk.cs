using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToucanAttk : MonoBehaviour
{
    //attk variables
    [SerializeField] float attkCooldown;
    float attkTimer;
    [SerializeField] Sprite[] droppables;
    [SerializeField] GameObject dropPoint;
    SpriteRenderer dropPointSR;
    bool holdingDroppable;
    int attkNum;


    // Start is called before the first frame update
    void Start()
    {
        dropPointSR = dropPoint.GetComponent<SpriteRenderer>();
        holdingDroppable = false;
    }

    // Update is called once per frame
    void Update()
    {
        attkTimer += Time.deltaTime;
        if(attkTimer >= attkCooldown && !holdingDroppable) {
            int random = Random.Range(0,4);
            dropPointSR.sprite = droppables[random];
            holdingDroppable = true;
            attkNum = random;
        }
        if(attkTimer >= attkCooldown + 2) {
            dropPointSR.sprite = null;
            holdingDroppable = false;
            attkTimer = 0;
        }

        //if attkTimer has reached attk cooldown, randomize an object, activate its sprite, and drop it shortly after actually instantiating the object
        //(alternatively just instantiate object once cool down has reached, but have it not fall until a bit after)
        //if attk type/num is the anvil do special dash

        //have seperate script for cocona as it doesn't just fall
    }

    void Attack() {

    }
}
