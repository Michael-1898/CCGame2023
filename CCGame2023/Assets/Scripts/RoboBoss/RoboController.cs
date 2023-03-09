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

    // Start is called before the first frame update
    void Start()
    {

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
            canonAngle = Random.Range(-150, 26);
            
            //sets rotate speed to rotate towards angle from correct direction
            if(canonPivot.rotation.z < canonAngle && canonRotateSpeed < 0) {
                canonRotateSpeed *= -1;
            }
            if(canonPivot.rotation.z > canonAngle && canonRotateSpeed > 0) {
                canonRotateSpeed *= -1;
            }
            canonShooting = true;
        }

        if(canonShooting && Mathf.Round(canonPivot.rotation.z) != canonAngle) { //if not at correct angle yet
            canonPivot.rotation = Quaternion.Euler(0f, 0f, canonAngle/*canonRotateSpeed * Time.deltaTime*/); //rotate towards correct angle
            print("rotating");
        } else if(Mathf.Round(canonPivot.rotation.z) == canonAngle && canonShooting) { //if at correct angle
            Instantiate(canonBalls[Random.Range(0,3)], canonPoint.position, Quaternion.identity);
            canonShotTimer = 0;
            canonShooting = false;
        }
    }

    void Attk2() { //ramming
        print("2");
    }

    void Attk3() { //missile shooting
        print("3");
    }
}
