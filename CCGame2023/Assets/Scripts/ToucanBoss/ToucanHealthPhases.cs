using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToucanHealthPhases : MonoBehaviour
{
    //phase variables
    [SerializeField] int[] phases;
    int currentPhase;

    //phase timers
    [SerializeField] float stunTime;
    float stunTimer;
    bool isStunned;
    [SerializeField] int maxDamage;
    int dmgRecieved;

    //scripts
    Health healthScript;
    ToucanBoss controllerScript;
    ToucanAttk attkScript;

    //components
    Collider2D col;
    Rigidbody2D rb;

    //knockback while stunned
    [SerializeField] float thrust;

    // Start is called before the first frame update
    void Start()
    {
        currentPhase = 0;

        healthScript = GetComponent<Health>();
        controllerScript = GetComponent<ToucanBoss>();
        attkScript = GetComponent<ToucanAttk>();

        col = GetComponent<PolygonCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentPhase < phases.Length && healthScript.currentHealth <= phases[currentPhase] && !isStunned) {
            //stun
            controllerScript.enabled = false;
            attkScript.enabled = false;
            col.isTrigger = false;
            isStunned = true;
            dmgRecieved--;
        }

        if(isStunned) {
            stunTimer += Time.deltaTime;

            if(healthScript.hit) {
                dmgRecieved++;

                //knockback while stunned
                if(dmgRecieved > 0) {
                    rb.AddForce(transform.up * thrust, ForceMode2D.Impulse);
                }
            }
            if(stunTimer >= stunTime || dmgRecieved >= maxDamage) {
                //unstun
                controllerScript.enabled = true;
                attkScript.enabled = true;
                col.isTrigger = true;
                isStunned = false;
                stunTimer = 0;
                dmgRecieved = 0;

                //move to next phase
                currentPhase++;
            }
        }
    }
}
