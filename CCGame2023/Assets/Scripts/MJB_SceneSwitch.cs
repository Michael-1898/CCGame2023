using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MJB_SceneSwitch : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject levelEditorMenu;
    GameObject levelEditorMenuBackground;
    
    void Start()
    {
        if(gameObject.name=="btnEditor")
        {
            print("Hi");
            levelEditorMenu = GameObject.Find("levelEditorMenu");
            levelEditorMenu.transform.position = new Vector3(0f, 200f, 0f);
            levelEditorMenu.SetActive(false);
            levelEditorMenuBackground = GameObject.Find("levelEditorMenuBackground");
            //levelEditorMenuBackground.transform.position = new Vector3(0f, 200f, 0f);
            levelEditorMenuBackground.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void moveToScene(int sceneID) {
        if(sceneID!=100)
        {
            SceneManager.LoadScene(sceneID);
        }
        else
        {
            levelEditorMenu.SetActive(true);
            levelEditorMenu.transform.position = new Vector3(0f, 0f, 0f);
            levelEditorMenuBackground.SetActive(true);
            levelEditorMenuBackground.transform.position = new Vector3(0f, 0f, 0f);

        }
    }
}