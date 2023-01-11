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
    public string levelPath;
    public Tilemap tilemap1;
    public List<Tile> allTiles = new List<Tile>();
    public List<string> allTileCharacters = new List<string>();
    [SerializeField] private Text levelNameText;
    [SerializeField] private string levelInformation;



    void Start()
    {
        PrintLevel(LoadLevel.LoadLevelFilePath);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PrintLevel(string levelName)
    {
        string fileName =  Application.streamingAssetsPath + "/" + levelName + ".txt";
            
        try
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

        
        for(int k = 0; k < levelInformation.Length; k++)
        {
            for(int l = 0; l < allTileCharacters.Count; l++)
            {
                if(levelInformation.Substring(k, 1) == allTileCharacters[l])
                {
                    tilemap1.SetTile(new Vector3Int(k % columns, Mathf.FloorToInt(k/columns), 0), allTiles[l]);
                }
            }
        }
    }
}
