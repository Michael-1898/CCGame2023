using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoboController : MonoBehaviour
{
    //variables for phase changes
    float attkPhaseTimer;
    [SerializeField] float[] attkPhaseLengths;
    int currentPhase;

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

    void Attk1() {
        print("1");
    }

    void Attk2() {
        print("2");
    }

    void Attk3() {
        print("3");
    }
}
