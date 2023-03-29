using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoboController : MonoBehaviour
{
    //variables for phase changes
    float attkPhaseTimer;
    [SerializeField] float[] attkPhaseLengths;
    int currentPhase;
    [SerializeField] float aggroRadius;
    bool aggroTaken;

    //variables for attk1
    float canonShotTimer;
    [SerializeField] float canonShotCooldown;
    [SerializeField] GameObject[] canonBalls;
    [SerializeField] Transform canonPoint;
    [SerializeField] Transform canonPivot;
    bool canonShooting;
    float canonAngle;
    [SerializeField] float canonRotateSpeed;
    int angleAdder;
    public Vector3 launchDir;
    [SerializeField] float launchForce;
    float angleMover;
    float priorAngle;

    //variables for attk2
    float ramTimer;
    [SerializeField] float ramCooldown;
    bool ramCharging;
    bool isRamming;
    Transform player;
    [SerializeField] float ramSpeed;
    Rigidbody2D rb;
    [SerializeField] float circleRadius;
    [SerializeField] GameObject edgeCheckL;
    [SerializeField] GameObject edgeCheckR;
    [SerializeField] GameObject wallCheckL;
    [SerializeField] GameObject wallCheckR;
    [SerializeField] LayerMask groundLayer;
    bool isHitting;
    [SerializeField] int enemyDmg;
    [SerializeField] float enemyKnockback;
    float ramChargeTimer;
    [SerializeField] float ramChargeTime;
    [SerializeField] GameObject smokeFX;
    [SerializeField] Transform smokePtL;
    [SerializeField] Transform smokePtR;
    bool smoked;

    //variables for attk3
    [SerializeField] GameObject missile;
    [SerializeField] float missileCooldown;
    float missileTimer;
    [SerializeField] Transform missilePt;
    float missileAngle;
    [SerializeField] float missileSpeed;

    // Start is called before the first frame update
    void Start()
    {
        aggroTaken = false;
        smoked = false;
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        priorAngle = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(!aggroTaken) {
            float distanceFromPlayer = Vector2.Distance(player.position, transform.position);
            if(distanceFromPlayer < aggroRadius) {
                aggroTaken = true;
            }
        }

        if(aggroTaken) {
            attkPhaseTimer += Time.deltaTime;
        }
        
        if(attkPhaseTimer >= attkPhaseLengths[currentPhase]) {
            currentPhase++;
            if(currentPhase == 1) {
                ramTimer = 0;
                GetComponent<RoboMovement>().enabled = true;
                smoked = false;
                ramChargeTimer = 0;
                isRamming = false;
                ramCharging = false;
            }
            attkPhaseTimer = 0;
            if(currentPhase > attkPhaseLengths.Length - 1) {
                currentPhase = 0;
            }
        }

        if(currentPhase == 0) {
            Attk1();
        }

        if(currentPhase == 1) {
            Attk2();
        }

        if(currentPhase == 2) {
            Attk3();
        }
    }

    void Attk1() { //canon shooting
        if(GetComponent<RoboMovement>().enabled == false) {
            GetComponent<RoboMovement>().enabled = true;
        }

        canonShotTimer += Time.deltaTime;
    
        if(canonShotTimer >= canonShotCooldown && !canonShooting) {
            canonAngle = Random.Range(-135, 14);
            if(canonAngle >= 0) {
                angleAdder = 0;
            } else {
                angleAdder = 360;
            }
            
            //sets rotate speed to rotate towards angle from correct direction
            if(priorAngle < canonAngle && canonRotateSpeed < 0) {
                canonRotateSpeed *= -1;
            }
            if(priorAngle > canonAngle && canonRotateSpeed > 0) {
                canonRotateSpeed *= -1;
            }
            canonShooting = true;
        }

        if(canonShooting) { //if not at correct angle yet
            canonPivot.rotation = Quaternion.Euler(0f, 0f, canonPivot.rotation.z + angleMover);
            angleMover += canonRotateSpeed;
        }
        if(Mathf.Round(canonPivot.localRotation.eulerAngles.z - angleAdder) == canonAngle && canonShooting) { //if at correct angle
            launchDir = (canonPoint.position - canonPivot.position).normalized;
            GameObject bomb = Instantiate(canonBalls[Random.Range(0,3)], canonPoint.position, Quaternion.identity);
            bomb.GetComponent<Rigidbody2D>().AddForce(launchDir * launchForce, ForceMode2D.Impulse);
            canonShotTimer = 0;
            priorAngle = canonPivot.localRotation.eulerAngles.z - angleAdder;
            canonShooting = false;
        }
    }

    void Attk2() { //ramming
        if(!isRamming) {
            ramTimer += Time.deltaTime;
        }

        if(ramTimer >= ramCooldown && !ramCharging && !isRamming && Physics2D.OverlapCircle(edgeCheckL.transform.position, circleRadius, groundLayer) == true && Physics2D.OverlapCircle(edgeCheckR.transform.position, circleRadius, groundLayer) == true) {
            GetComponent<RoboMovement>().enabled = false;

            //sets movement direction towards player
            if(player.position.x < transform.position.x && !smoked) {
                Instantiate(smokeFX, smokePtR.position, Quaternion.Euler(0, 0, -25.5f));
                smoked = true;
            } else if(player.position.x >= transform.position.x && !smoked) {
                Instantiate(smokeFX, smokePtL.position, Quaternion.Euler(0, 0, 25.5f));
                smoked = true;
            }

            //sets rotate speed to rotate towards angle from correct direction
            if(priorAngle < 50 && canonRotateSpeed < 0) {
                canonRotateSpeed *= -1;
            }
            if(priorAngle > 50 && canonRotateSpeed > 0) {
                canonRotateSpeed *= -1;
            }

            ramCharging = true;
        }

        if(ramCharging && Mathf.Round(canonPivot.localRotation.eulerAngles.z) != 50) {
            canonPivot.rotation = Quaternion.Euler(0f, 0f, canonPivot.rotation.z + angleMover);
            angleMover += canonRotateSpeed;
            priorAngle = priorAngle = canonPivot.localRotation.eulerAngles.z;
        }
        if(ramCharging && ramChargeTimer < ramChargeTime) {
            ramChargeTimer += Time.deltaTime;
        }

        if(Mathf.Round(canonPivot.localRotation.eulerAngles.z) == 50 && ramCharging && ramChargeTimer >= ramChargeTime) {
            //sets movement direction towards player
            if(player.position.x < transform.position.x && ramSpeed > 0) {
                ramSpeed *= -1;
                print("dir change");
            } else if(player.position.x >= transform.position.x && ramSpeed < 0) {
                ramSpeed *= -1;
                print("dir change");
            }

            isRamming = true;
            ramCharging = false;
        }

        if(isRamming) {
            rb.velocity = Vector2.right * ramSpeed * (Time.deltaTime + 1);
            //if hitting a wall, or off an edge
            if(Physics2D.OverlapCircle(edgeCheckL.transform.position, circleRadius, groundLayer) == false || Physics2D.OverlapCircle(edgeCheckR.transform.position, circleRadius, groundLayer) == false || Physics2D.OverlapCircle(wallCheckL.transform.position, circleRadius, groundLayer) == true || Physics2D.OverlapCircle(wallCheckR.transform.position, circleRadius, groundLayer) == true || isHitting) {
                ramTimer = 0;
                GetComponent<RoboMovement>().enabled = true;
                smoked = false;
                ramChargeTimer = 0;
                isRamming = false;
            }
        }
    }

    void Attk3() { //missile shooting
        if(GetComponent<RoboMovement>().enabled == true) {
            GetComponent<RoboMovement>().enabled = false;
        }

        missileTimer += Time.deltaTime;
        if(missileTimer >= missileCooldown) {
            //fire missile
            missileAngle = Random.Range(-1f, 10f); //sets random angle for missiel to spawn at, so they don't all take same path up
            GameObject newMissile = Instantiate(missile, missilePt.position, Quaternion.Euler(0, 0, missileAngle));
            newMissile.GetComponent<Rigidbody2D>().velocity = (newMissile.transform.GetChild(0).transform.position - newMissile.transform.GetChild(1).transform.position).normalized * missileSpeed * (Time.deltaTime + 1);

            missileTimer = 0;
        }
    }

    void OnCollisionEnter2D(Collision2D col) {
        if(col.gameObject.CompareTag("Player") && col.gameObject.GetComponent<MJB_PlayerMove>().kbCurrentTime <= 0) {   //if collided with player
            //deal damage
            col.gameObject.GetComponent<PlayerHealth>().TakeDamage(enemyDmg);

            //set kb time for player
            col.gameObject.GetComponent<MJB_PlayerMove>().kbCurrentTime = col.gameObject.GetComponent<MJB_PlayerMove>().kbTotalTime;

            if(transform.position.x < col.transform.position.x) {   //if player is on right
                col.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.right * enemyKnockback * (Time.deltaTime + 1);
            } else {    //if player is on left
                col.gameObject.GetComponent<Rigidbody2D>().velocity = -Vector2.right * enemyKnockback * (Time.deltaTime + 1);
            }

            isHitting = true;
        }
    }

    void OnCollisionExit2D(Collision2D col) {
        if(isHitting) {
            isHitting = false;
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(edgeCheckL.transform.position, circleRadius);
        Gizmos.DrawWireSphere(edgeCheckR.transform.position, circleRadius);
        Gizmos.DrawWireSphere(wallCheckL.transform.position, circleRadius);
        Gizmos.DrawWireSphere(wallCheckR.transform.position, circleRadius);
        Gizmos.DrawWireSphere(transform.position, aggroRadius);
    }
}
