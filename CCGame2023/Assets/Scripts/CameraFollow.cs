using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 offset = new Vector3(0f, 3f, -10f);
    private float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;

    [SerializeField] private Transform target;

    // Start is called before the first frame update
    void Start()
    {
        if(target == null) {
            target = GameObject.Find("Player(Clone)").transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = target.position + offset;
                            //Vector3.SmoothDamp(current position, target position, current velocity, time it takes to reach target)
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);    
    }
}
