﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class tilemap : MonoBehaviour
{
    // Start is called before the first frame update
    public Tilemap tilemap1;
    public static List<Tile> allTiles = new List<Tile>();
    public static List<string> allTileCharacters = new List<string>();
    public static List<GameObject> allTileGameObjects = new List<GameObject>();
    public static List<Vector3> allTileSizes = new List<Vector3>();
    public static List<int> allTileLimits = new List<int>();

    public List<int> allTileCurrentNumbers = new List<int>();
    public List<Button> allTileButtons = new List<Button>();
    [SerializeField] private Text levelNameText;
    [SerializeField] private Text creatorNameText;
    [SerializeField] private string levelInformation;
    [SerializeField] private Tile xTile;
    [SerializeField] private Tile deleteTile;
    [SerializeField] private Tile rectangleToolTile;
    public GameObject tilePreview;
    SpriteRenderer tilePreviewSR;
    Tile currentTile;
    int currentTileIndex;
    Vector3Int currentDeletionLocation;
    Vector3 currentTileSize;
    bool delete = false;
    bool rectangleTool = false;
    bool rectangleToolFirstClick = false;

    public int columns;
    public int rows;
    

    
    
    void Start()
    {
        tilePreviewSR = tilePreview.GetComponent<SpriteRenderer>();
        Cursor.lockState = CursorLockMode.Confined;

        print(allTileCharacters.Count + " YOULOUIHN");
        print(allTileLimits.Count + " yugebosh");
        for (int i = 0; i < allTileLimits.Count; i++)
        {
            allTileCurrentNumbers.Add(0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(currentTile != null && (Camera.main.ScreenToWorldPoint(Input.mousePosition).x < (Camera.main.orthographicSize * (16f/9f))-6.2f+Camera.main.transform.position.x))
        {
            
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            tilePreview.transform.position = new Vector3(Mathf.Floor(mousePos.x)+0.5f, Mathf.Floor(mousePos.y)+0.5f, 0f);
            if(Input.GetKeyDown("escape"))
            {
                print("escape");
                currentTile = null;
                tilePreviewSR.sprite = null;
                delete=false;
            }
            if (currentTileSize != new Vector3(1f, 1f, 0f) && delete == false && rectangleTool == false)
            {
                tilePreviewSR.sprite = currentTile.sprite;
                bool noXTiles = true;
                for (int w = 0; w < (currentTileSize.x * currentTileSize.y); w++)
                {
                    if (tilemap1.GetTile(new Vector3Int(Mathf.FloorToInt((tilePreview.transform.position.x - 0.5f) + (w % currentTileSize.x)), Mathf.FloorToInt(tilePreview.transform.position.y - 0.5f) + Mathf.FloorToInt(w / currentTileSize.x), 0)) == xTile)
                    {
                        noXTiles = false;
                    }
                }
                if (noXTiles)
                {
                   
                    if(allTileLimits[currentTileIndex] > allTileCurrentNumbers[currentTileIndex])
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            GameObject.Find("Warning Text").GetComponent<Text>().text = "";
                            if (tilemap1.GetTile(new Vector3Int(Mathf.FloorToInt(tilePreview.transform.position.x - 0.5f), Mathf.FloorToInt(tilePreview.transform.position.y - 0.5f), 0)) != xTile)
                            {
                                currentDeletionLocation = new Vector3Int(Mathf.FloorToInt(tilePreview.transform.position.x - 0.5f), Mathf.FloorToInt(tilePreview.transform.position.y - 0.5f), 0);
                                deleteXTiles();
                                tilemap1.SetTile(new Vector3Int(Mathf.FloorToInt(tilePreview.transform.position.x - 0.5f), Mathf.FloorToInt(tilePreview.transform.position.y - 0.5f), 0), currentTile);
                                allTileCurrentNumbers[currentTileIndex]++;
                            }
                            for (int m = 1; m < (currentTileSize.x * currentTileSize.y); m++)
                            {
                                if (tilemap1.GetTile(new Vector3Int(Mathf.FloorToInt((tilePreview.transform.position.x - 0.5f) + (m % currentTileSize.x)), Mathf.FloorToInt(tilePreview.transform.position.y - 0.5f) + Mathf.FloorToInt(m / currentTileSize.x), 0)) != xTile)
                                {
                                    currentDeletionLocation = new Vector3Int(Mathf.FloorToInt((tilePreview.transform.position.x - 0.5f) + (m % currentTileSize.x)), Mathf.FloorToInt(tilePreview.transform.position.y - 0.5f) + Mathf.FloorToInt(m / currentTileSize.x), 0);
                                    deleteXTiles();
                                }
                                tilemap1.SetTile(new Vector3Int(Mathf.FloorToInt((tilePreview.transform.position.x - 0.5f) + (m % currentTileSize.x)), Mathf.FloorToInt(tilePreview.transform.position.y - 0.5f) + Mathf.FloorToInt(m / currentTileSize.x), 0), xTile);
                            }
                        }
                    }
                    else
                    {
                        GameObject.Find("Warning Text").GetComponent<Text>().text = "Maximum number of the object/tile";
                    }
                           
                   
                }
            }
            else if(delete == false && rectangleTool == false)
            {
                tilePreviewSR.sprite = currentTile.sprite;
          
                if(allTileLimits[currentTileIndex] > allTileCurrentNumbers[currentTileIndex])
                {
                    GameObject.Find("Warning Text").GetComponent<Text>().text = "";
                    if(Input.GetMouseButton(0))
                    {
                        if (tilemap1.GetTile(new Vector3Int(Mathf.FloorToInt(tilePreview.transform.position.x - 0.5f), Mathf.FloorToInt(tilePreview.transform.position.y - 0.5f), 0))!=xTile && tilemap1.GetTile(new Vector3Int(Mathf.FloorToInt(tilePreview.transform.position.x - 0.5f), Mathf.FloorToInt(tilePreview.transform.position.y - 0.5f), 0)) != currentTile)
                        {
                            currentDeletionLocation = new Vector3Int(Mathf.FloorToInt(tilePreview.transform.position.x - 0.5f), Mathf.FloorToInt(tilePreview.transform.position.y - 0.5f), 0);
                            deleteXTiles();
                            tilemap1.SetTile(new Vector3Int(Mathf.FloorToInt(tilePreview.transform.position.x - 0.5f), Mathf.FloorToInt(tilePreview.transform.position.y - 0.5f), 0), currentTile);
                            allTileCurrentNumbers[currentTileIndex]++;
                        }
                    }
                        
                }
                else GameObject.Find("Warning Text").GetComponent<Text>().text = "Maximum number of the object/tile";

                
            }
            else if(delete == true)
            {

                tilePreviewSR.sprite = GameObject.Find("delete").GetComponent<Image>().sprite;
                if (Input.GetMouseButton(0))
                {
                    if (tilemap1.GetTile(new Vector3Int(Mathf.FloorToInt(tilePreview.transform.position.x - 0.5f), Mathf.FloorToInt(tilePreview.transform.position.y - 0.5f), 0))!=xTile)
                    {
                        currentDeletionLocation = new Vector3Int(Mathf.FloorToInt(tilePreview.transform.position.x - 0.5f), Mathf.FloorToInt(tilePreview.transform.position.y - 0.5f), 0);
                        deleteXTiles();

                        TileBase tempTile = tilemap1.GetTile(new Vector3Int(Mathf.FloorToInt(tilePreview.transform.position.x - 0.5f), Mathf.FloorToInt(tilePreview.transform.position.y - 0.5f), 0));
                        tilemap1.SetTile(new Vector3Int(Mathf.FloorToInt(tilePreview.transform.position.x - 0.5f), Mathf.FloorToInt(tilePreview.transform.position.y - 0.5f), 0), null);
                        
                        for(int z = 0; z < allTileCharacters.Count; z++)
                        {
                            if(allTiles[z] == tempTile)
                            {
                                allTileCurrentNumbers[z]--;
                            }
                        }
                        
                    }
                }
            }
            else if(rectangleTool == true)
            {
                tilePreviewSR.sprite = GameObject.Find("rectangleTool").GetComponent<Image>().sprite;

                /*








                WORK HERE NEXT TIME








                */
            }
        }
        else
        {
            tilePreviewSR.sprite = null;
        }
            

        if(Input.GetKeyDown("p"))
        {
            print(getTilemapInformation(columns, rows));
        }
    }

    public void deleteXTiles()
    {
        for (int temp = 1; temp < tilemap.allTiles.Count; temp++)
        {
            if (tilemap1.GetTile(currentDeletionLocation) == tilemap.allTiles[temp] && tilemap1.GetTile(currentDeletionLocation) != null && tilemap1.GetTile(currentDeletionLocation) != xTile)
            {
                print(tilemap.allTiles[temp]);
                allTileCurrentNumbers[temp]--;
                for (int temp2 = 1; temp2 < (tilemap.allTileSizes[temp].x * tilemap.allTileSizes[temp].y); temp2++)
                {
                    print(new Vector3Int(Mathf.FloorToInt(currentDeletionLocation.x + (temp2 % tilemap.allTileSizes[temp].x)), currentDeletionLocation.y + Mathf.FloorToInt(temp2 / tilemap.allTileSizes[temp].x), 0));
                    tilemap1.SetTile(new Vector3Int(Mathf.FloorToInt(currentDeletionLocation.x + (temp2 % tilemap.allTileSizes[temp].x)), currentDeletionLocation.y + Mathf.FloorToInt(temp2 / tilemap.allTileSizes[temp].x), 0), null);
                }
            }
        }
    }
    public void printTilemapInformation(string levelName)
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
                if (levelInformation.Substring(k, 1) == allTileCharacters[l])
                {
                    tilemap1.SetTile(new Vector3Int(k % columns, Mathf.FloorToInt(k/columns), 0), allTiles[l]);
                }
            }
        }
    }

    public string getTilemapInformation(int columns, int rows)
    {
        string tileInformation = "";
        
        for(int i = 0; i < (columns * rows); i++)
        {
            TileBase currentTile = tilemap1.GetTile(new Vector3Int(i % columns, Mathf.FloorToInt(i/columns), 0));
            for(int j = 0; j < allTiles.Count; j++)
            {
                if(currentTile == allTiles[j])
                {
                    tileInformation += allTileCharacters[j];
                }
                
            }
            if(currentTile == xTile)
            {
                tileInformation += "a";
            }
        }
        return(tileInformation);
    }

    public void buttonClicked(string name)
    {
        for(int i = 0; i < allTileCharacters.Count; i++)
        {
            if(allTileCharacters[i] == name)
            {
                currentTile = allTiles[i];
                currentTileSize = allTileSizes[i];
                currentTileIndex = i;
                delete = false;
                rectangleTool = false;
            }
        }
        if(name == "delete")
        {
            delete = true;
            rectangleTool = false;
            currentTile = deleteTile;
            currentTileIndex = 0;
            tilePreviewSR.sprite = GameObject.Find("delete").GetComponent<Image>().sprite;
        }
        if(name == "rectangleTool")
        {
            rectangleTool = true;
            delete = false;
            currentTile = rectangleToolTile;
            currentTileIndex = 0;
            tilePreviewSR.sprite = GameObject.Find("rectangleTool").GetComponent<Image>().sprite;
        }
        if(name == "CreateLevelButton")
        {
            string nameOfLevel = levelNameText.text;
            string nameOfCreator = creatorNameText.text;
            print(creatorNameText.text);
            if (nameOfLevel != "" && nameOfCreator != "")
            {
                if(allTileCurrentNumbers[5] == 1)
                {
                    saveLevel(nameOfLevel, nameOfCreator);
                }
                else GameObject.Find("Warning Text").GetComponent<Text>().text = "Please place down the character";
            }
            else GameObject.Find("Warning Text").GetComponent<Text>().text = "Fill in both name of level and creator name";
        }
    }

    public void saveLevel(string levelName, string creatorName)
    {
        bool fileExists = false;
        string fileName =  Application.streamingAssetsPath + "/" + levelName + ".txt";

        DirectoryInfo dir = new DirectoryInfo(Application.streamingAssetsPath);
        FileInfo[] info = dir.GetFiles("*.*");
        foreach(FileInfo i in info)
        {
            if(File.Exists(fileName))
            {
                fileExists = true;
                GameObject.Find("Warning Text").GetComponent<Text>().text = "There is already a level named that. Please rename your level";
            }
        }

        if(!fileExists)
        {
            FileStream stream = null;
            try
            {
                stream = new FileStream(fileName, FileMode.OpenOrCreate); 
                using (StreamWriter writer = new StreamWriter(stream))  
                {  
                    writer.WriteLine(creatorName);
                    writer.WriteLine(columns);
                    writer.WriteLine(rows);
                    writer.WriteLine(getTilemapInformation(columns, rows));  
                }  
                SceneManager.LoadScene("LevelSelection");
            }
            catch
            {
                print("L");
            }
        }
    }
}
