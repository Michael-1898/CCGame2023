using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MJB_SceneSwitch : MonoBehaviour
{
    // Start is called before the first frame update
    
    void Start()
    {
        if(gameObject.name=="btnEditor")
        {
            print("Hi");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("escape") && gameObject.name == "btnEditor")
        {
            Camera.main.transform.position = new Vector3(0f, 0f, 0f);
        }
    }

    public void moveToScene(int sceneID) {
        if(sceneID!=100)
        {
            SceneManager.LoadScene(sceneID);
        }
        else
        {
            Camera.main.transform.position = new Vector3(30f, 0f, -10f);
        }
    }
}