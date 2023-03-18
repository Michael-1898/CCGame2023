using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditorController : MonoBehaviour
{
    // Start is called before the first frame update
    Camera cam; 
    public int columns;
    public int rows;

    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey("w") || Input.GetKey("up"))
        {
            transform.position += new Vector3(0f, 0.1f, 0f);
        }
        if(Input.GetKey("a") || Input.GetKey("left"))
        {
            transform.position += new Vector3(-0.1f, 0f, 0f);
        }
        if(Input.GetKey("s") || Input.GetKey("down"))
        {
            transform.position += new Vector3(0f, -0.1f, 0f);
        }
        if(Input.GetKey("d") || Input.GetKey("right"))
        {
            transform.position += new Vector3(0.1f, 0f, 0f);
        }

        if(transform.position.x - (cam.orthographicSize * (16f/9f)) < -4)
        {
            transform.position += new Vector3(0.1f, 0f, 0f);
        }
        if(transform.position.y - (cam.orthographicSize) < -4)
        {
            transform.position += new Vector3(0f, 0.1f, 0f);
        }
        if(transform.position.x + (cam.orthographicSize * (16f/9f)) > (columns+10.25f))
        {
            transform.position += new Vector3(-0.1f, 0f, 0f);
        }
        if(transform.position.y + (cam.orthographicSize) > (rows+4f))
        {
            transform.position += new Vector3(0f, -0.1f, 0f);
        }
    }
}