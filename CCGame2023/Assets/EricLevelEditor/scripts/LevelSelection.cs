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
    int numberOfPanels = 0;

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

                Text levelName = newPanel.transform.GetChild(0).gameObject.GetComponent<Text>();
                levelName.text = i.Name.Substring(0, i.Name.Length-4);
                try
                { 
                    
                    string fileName =  i.DirectoryName + "\\" + i.Name;
                    using (StreamReader reader = new StreamReader(fileName))
                    {
                        string creatorName = reader.ReadLine();
                        newPanel.transform.GetChild(1).GetChild(0).gameObject.GetComponent<Text>().text = creatorName;
                    }
                    
                        
                        
                    

                }
                catch
                {
                    print("Failed to read creator name");
                }
                numberOfPanels++;
                /*try
                { 
                    using (StreamReader reader = new StreamReader(fileName))  
                    {  
                        columns = int.Parse(reader.ReadLine());
                        rows = int.Parse(reader.ReadLine());
                        levelInformation = reader.ReadLine();
                    }  

                }
                catch
                {
                    print("This file can't be read");
                }
                */
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if((Input.GetKey("left") || Input.GetKey("a")) && transform.position.x > 0f)
        {
            transform.position += new Vector3(-0.1f, 0f, 0f);
        }
        if ((Input.GetKey("right") || Input.GetKey("d")) && transform.position.x < Mathf.Floor(((numberOfPanels-1)/2)-2)*5)
        {
            transform.position += new Vector3(0.1f, 0f, 0f);
        }
    }
}
