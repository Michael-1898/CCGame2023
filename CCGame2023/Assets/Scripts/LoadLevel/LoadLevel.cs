using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using System.IO;


public class LoadLevel : MonoBehaviour
{
    // Start is called before the first frame update
    public static string LoadLevelFilePath;
    
    int columns;
    int rows;
    string levelPath;
    public Tilemap tilemap1;
    private string levelInformation;



    void Start()
    {
        PrintLevel(LoadLevelFilePath);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PrintLevel(string levelName)
    {
        string fileName =  Application.streamingAssetsPath + "/" + levelName;
        print(fileName);
        try
        { 
            using (StreamReader reader = new StreamReader(fileName))  
            {  
                columns = int.Parse(reader.ReadLine());
                rows = int.Parse(reader.ReadLine());
                print(rows);
                levelInformation = reader.ReadLine();
                print(levelInformation);
            }  

        }
        catch
        {
            print("This file can't be read");
        }

        
        for(int k = 0; k < levelInformation.Length; k++)
        {
            for(int l = 0; l < tilemap.allTileCharacters.Count; l++)
            {
            
                if (levelInformation.Substring(k, 1) == tilemap.allTileCharacters[l])
                {
                    tilemap1.SetTile(new Vector3Int(k % columns, Mathf.FloorToInt(k / columns), 0), tilemap.allTiles[l]);
                    if(tilemap.allTileGameObjects[l] != null)
                    {
                        tilemap1.SetTile(new Vector3Int(k % columns, Mathf.FloorToInt(k / columns), 0), null);
                        Instantiate(tilemap.allTileGameObjects[l], new Vector3((k % columns) + 0.5f, Mathf.FloorToInt(k / columns) + 0.5f, 0), Quaternion.identity);
                    }
                }
            }
        }
    }
}
