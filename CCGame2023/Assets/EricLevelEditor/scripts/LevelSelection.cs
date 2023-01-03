using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelection : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DirectoryInfo dir = new DirectoryInfo(Application.streamingAssetsPath);
        FileInfo[] info = dir.GetFiles("*.*");
        foreach(FileInfo i in info)
        {
            

            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
