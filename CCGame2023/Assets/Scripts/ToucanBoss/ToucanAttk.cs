using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToucanAttk : MonoBehaviour
{
    //attk timing variables
    [SerializeField] float attkCooldown;
    float attkTimer;
    [SerializeField] float holdTime;

    //droppable variables
    [SerializeField] Sprite[] droppables;
    [SerializeField] GameObject dropPoint;
    SpriteRenderer dropPointSR;
    bool holdingDroppable;

    //attk type variables    
    int objectNum;
    Rigidbody2D rb;
    [SerializeField] float dashSpeed;
    bool isDashing;
    bool isFacingRight;

    //dropabbles
    [SerializeField] GameObject[] dropObjects;


    // Start is called before the first frame update
    void Start()
    {
        dropPointSR = dropPoint.GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        holdingDroppable = false;
        isDashing = false;
    }

    // Update is called once per frame
    void Update()
    {
        //attk holding timer
        attkTimer += Time.deltaTime;

        //makes toucan face correct direction
        if(rb.velocity.x > 0 && !isFacingRight) {
            Flip();
        } else if(rb.velocity.x < 0 && isFacingRight) {
            Flip();
        }

        if(attkTimer >= attkCooldown && !holdingDroppable) {
            //chooses object randomly
            objectNum = Random.Range(0,4);
            objectNum = 3;

            dropPointSR.sprite = droppables[objectNum];
            holdingDroppable = true;
        }
        if(attkTimer >= attkCooldown + holdTime) {
            dropPointSR.sprite = null;
            holdingDroppable = false;
            Instantiate(dropObjects[objectNum], dropPoint.transform.position, Quaternion.identity);
            rb.velocity = Vector2.zero;
            attkTimer = 0;
        }

        
        //attk variations
        if(objectNum == 3 && attkTimer < attkCooldown + holdTime && holdingDroppable) {
            GameObject target = GameObject.FindGameObjectWithTag("Player");
            Vector3 targetTemp = new Vector2(target.transform.position.x - 0.8f, target.transform.position.y + 7.5f);
            Vector2 moveDir = (targetTemp - transform.position).normalized * dashSpeed;
            rb.velocity = new Vector2(moveDir.x, moveDir.y);
            if(!isDashing) {
                isDashing = true;
            }

            //if toucan gets close enough, drop anvil
            if(isDashing && transform.position.x - target.transform.position.x >= -1.1f && transform.position.x - target.transform.position.x <= -0.8f) {
                attkTimer = attkCooldown + holdTime;
                isDashing = false;
            }
        }

        void Flip() {
            isFacingRight = !isFacingRight;
            transform.Rotate(new Vector3(0, 180, 0));
        }
    }
}
