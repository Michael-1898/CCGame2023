using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;


public class LevelSelection : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject levelPanel;
    Vector3 newPanelPosition = new Vector3(-6f, 2.5f, 0f);
    [SerializeField] RectTransform canvas;

    void Start()
    {
        DirectoryInfo dir = new DirectoryInfo(Application.streamingAssetsPath);
        FileInfo[] info = dir.GetFiles("*.*");
        foreach(FileInfo i in info)
        {
            if(i.Name.Substring(i.Name.Length-4, 4)!="meta")
            {
                GameObject newPanel = Instantiate(levelPanel, newPanelPosition, Quaternion.identity);
                newPanel.transform.parent = canvas;
                if(newPanelPosition.y < 0f)
                {
                    newPanelPosition = new Vector3(newPanelPosition.x + 5f, 2.5f, 0f);
                }
                else newPanelPosition = new Vector3(newPanelPosition.x, -2f, 0f);
                
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
