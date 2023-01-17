using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZquitController : MonoBehaviour
{
    [SerializeField] float aggroRadius;
    [SerializeField] float followRadius;
    [SerializeField] float moveSpeed;

    [SerializeField] LayerMask playerLayer;
    [SerializeField] Transform player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);
        if(distanceFromPlayer < aggroRadius && distanceFromPlayer > followRadius) {
            transform.position = Vector2.MoveTowards(this.transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, aggroRadius);
        Gizmos.DrawWireSphere(transform.position, followRadius);
    }
}
