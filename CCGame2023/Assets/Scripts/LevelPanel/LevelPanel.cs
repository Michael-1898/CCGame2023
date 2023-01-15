using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelPanel : MonoBehaviour
{
    // Start is called before the first frame update
    
    
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playLevel()
    {
        LoadLevel.LoadLevelFilePath = gameObject.transform.GetChild(0).GetComponent<Text>().text + ".txt";
        SceneManager.LoadScene("TestGame");
    }
}
