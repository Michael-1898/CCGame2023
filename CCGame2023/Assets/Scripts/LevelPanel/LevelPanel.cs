using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        LoadLevel.LoadLevelFilePath = gameObject.name + ".txt";
        SceneManager.LoadScene("TestGame");
    }
}
