using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombLauncher : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    Vector3 launchDir;
    [SerializeField] GameObject roboBoss;
    [SerializeField] float launchForce;

    // Start is called before the first frame update
    void Start()
    {
        launchDir = (transform.position - roboBoss.transform.GetChild(7).transform.position).normalized;
        rb.AddForce(launchDir * launchForce, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
