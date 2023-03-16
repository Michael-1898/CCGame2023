using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoboController : MonoBehaviour
{
    //variables for phase changes
    float attkPhaseTimer;
    [SerializeField] float[] attkPhaseLengths;
    int currentPhase;

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

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        attkPhaseTimer += Time.deltaTime;
        if(attkPhaseTimer >= attkPhaseLengths[currentPhase]) {
            currentPhase++;
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

        if(ramTimer >= ramCooldown && !ramCharging) {
            ramCharging = true;

            //sets rotate speed to rotate towards angle from correct direction
            if(priorAngle < 50 && canonRotateSpeed < 0) {
                canonRotateSpeed *= -1;
            }
            if(priorAngle > 50 && canonRotateSpeed > 0) {
                canonRotateSpeed *= -1;
            }
        }

        if(ramCharging && Mathf.Round(canonPivot.localRotation.eulerAngles.z) != 50) {
            canonPivot.rotation = Quaternion.Euler(0f, 0f, canonPivot.rotation.z + angleMover);
            angleMover += canonRotateSpeed;
        }

        if(Mathf.Round(canonPivot.localRotation.eulerAngles.z) == 50 && ramCharging) {
            GetComponent<RoboMovement>().enabled = false;

            //sets movement direction towards player
            if(player.position.x < transform.position.x && ramSpeed > 0) {
                ramSpeed *= -1;
            } else if(player.position.x >= transform.position.x && ramSpeed < 0) {
                ramSpeed *= -1;
            }

            isRamming = true;
            ramCharging = false;
        }

        if(isRamming) {
            
            float distanceFromPlayer = Vector2.Distance(player.position, transform.position);

            rb.velocity = Vector2.right * ramSpeed * (Time.deltaTime + 1);
            //if hitting a wall, or off an edge
            if(Physics2D.OverlapCircle(edgeCheckL.transform.position, circleRadius, groundLayer) == false || Physics2D.OverlapCircle(edgeCheckR.transform.position, circleRadius, groundLayer) == false || Physics2D.OverlapCircle(wallCheckL.transform.position, circleRadius, groundLayer) == true || Physics2D.OverlapCircle(wallCheckR.transform.position, circleRadius, groundLayer) == true) {
                print("ram ended");
                ramTimer = 0;
                GetComponent<RoboMovement>().enabled = false;
                isRamming = false;
            }
        }
    }

    void Attk3() { //missile shooting
        print("3");
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(edgeCheckL.transform.position, circleRadius);
        Gizmos.DrawWireSphere(edgeCheckR.transform.position, circleRadius);
        Gizmos.DrawWireSphere(wallCheckL.transform.position, circleRadius);
        Gizmos.DrawWireSphere(wallCheckR.transform.position, circleRadius);
    }
}
