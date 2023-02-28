using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardoAttack : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 1000; i++)
        {
            Attack();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Attack()
    {
        int random = Mathf.FloorToInt(Random.Range(1,5));
        print(random);
    }
}
