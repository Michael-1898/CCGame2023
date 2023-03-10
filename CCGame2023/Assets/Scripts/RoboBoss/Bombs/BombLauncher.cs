using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombLauncher : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    Vector3 launchDir;
    [SerializeField] GameObject roboBoss;
    [SerializeField] float launchForce;

    bool once;

    // Start is called before the first frame update
    void Start()
    {
        once = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!once) {
            launchDir = (roboBoss.transform.GetChild(7).transform.GetChild(1).transform.position - roboBoss.transform.GetChild(7).transform.position).normalized;
            rb.AddForce(launchDir * launchForce, ForceMode2D.Impulse);
            print("new angle");

            once = true;
        }

        //have the direction determined outside of prefab script so that it changes
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(roboBoss.transform.GetChild(7).transform.GetChild(1).transform.position, 0.2f);
        Gizmos.DrawWireSphere(roboBoss.transform.GetChild(7).transform.position, 0.2f);
    }
}
