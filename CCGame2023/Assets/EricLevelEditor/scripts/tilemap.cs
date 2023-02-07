using System.Collections;
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

    public List<Button> allTileButtons = new List<Button>();
    [SerializeField] private Text levelNameText;
    [SerializeField] private string levelInformation;
    [SerializeField] private Tile xTile;
    [SerializeField] private Tile deleteTile;
    public GameObject tilePreview;
    SpriteRenderer tilePreviewSR;
    Tile currentTile;
    Vector3Int currentDeletionLocation;
    Vector3 currentTileSize;
    bool delete = false;

    public int columns;
    public int rows;
    

    
    
    void Start()
    {
        tilePreviewSR = tilePreview.GetComponent<SpriteRenderer>();
        Cursor.lockState = CursorLockMode.Confined;
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
            if (currentTileSize != new Vector3(1f, 1f, 0f) && delete == false)
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
                    if (Input.GetMouseButtonDown(0))
                    {

                        if (tilemap1.GetTile(new Vector3Int(Mathf.FloorToInt(tilePreview.transform.position.x - 0.5f), Mathf.FloorToInt(tilePreview.transform.position.y - 0.5f), 0)) != xTile)
                        {
                            currentDeletionLocation = new Vector3Int(Mathf.FloorToInt(tilePreview.transform.position.x - 0.5f), Mathf.FloorToInt(tilePreview.transform.position.y - 0.5f), 0);
                            deleteXTiles();
                            tilemap1.SetTile(new Vector3Int(Mathf.FloorToInt(tilePreview.transform.position.x - 0.5f), Mathf.FloorToInt(tilePreview.transform.position.y - 0.5f), 0), currentTile);
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
            }
            else if(delete == false)
            {
                print(currentTile.name);
                tilePreviewSR.sprite = currentTile.sprite;
                if (Input.GetMouseButton(0))
                {
                    if (tilemap1.GetTile(new Vector3Int(Mathf.FloorToInt(tilePreview.transform.position.x - 0.5f), Mathf.FloorToInt(tilePreview.transform.position.y - 0.5f), 0))!=xTile)
                    {
                        currentDeletionLocation = new Vector3Int(Mathf.FloorToInt(tilePreview.transform.position.x - 0.5f), Mathf.FloorToInt(tilePreview.transform.position.y - 0.5f), 0);
                        deleteXTiles();
                        tilemap1.SetTile(new Vector3Int(Mathf.FloorToInt(tilePreview.transform.position.x - 0.5f), Mathf.FloorToInt(tilePreview.transform.position.y - 0.5f), 0), currentTile);
                    }
                }
            }
            else
            {

                tilePreviewSR.sprite = GameObject.Find("delete").GetComponent<Image>().sprite;
                if (Input.GetMouseButton(0))
                {
                    if (tilemap1.GetTile(new Vector3Int(Mathf.FloorToInt(tilePreview.transform.position.x - 0.5f), Mathf.FloorToInt(tilePreview.transform.position.y - 0.5f), 0))!=xTile)
                    {
                        currentDeletionLocation = new Vector3Int(Mathf.FloorToInt(tilePreview.transform.position.x - 0.5f), Mathf.FloorToInt(tilePreview.transform.position.y - 0.5f), 0);
                        deleteXTiles();
                        tilemap1.SetTile(new Vector3Int(Mathf.FloorToInt(tilePreview.transform.position.x - 0.5f), Mathf.FloorToInt(tilePreview.transform.position.y - 0.5f), 0), null);
                    }
                }
            }
        }
        else
        {
            tilePreviewSR.sprite = null;
            print("uoaef");
        }
            

        if(Input.GetKeyDown("p"))
        {
            print(getTilemapInformation(columns, rows));
        }
        if(Input.GetKeyDown(";"))
        {
            string nameOfLevel = levelNameText.text;
            saveLevel(nameOfLevel);
        }
    }

    public void deleteXTiles()
    {
        for (int temp = 1; temp < tilemap.allTiles.Count; temp++)
        {
            if (tilemap1.GetTile(currentDeletionLocation) == tilemap.allTiles[temp] && tilemap1.GetTile(currentDeletionLocation) != null && tilemap1.GetTile(currentDeletionLocation) != xTile)
            {
                print(tilemap.allTiles[temp]);
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
                delete = false;
            }
        }
        if(name == "delete")
        {
            delete = true;
            currentTile = deleteTile;
            tilePreviewSR.sprite = GameObject.Find("delete").GetComponent<Image>().sprite;
        }
    }

    public void saveLevel(string levelName)
    {
        bool fileExists = false;
        string fileName =  Application.streamingAssetsPath + "/" + levelName + ".txt";
        print(fileName + "        this is me");       

        DirectoryInfo dir = new DirectoryInfo(Application.streamingAssetsPath);
        FileInfo[] info = dir.GetFiles("*.*");
        foreach(FileInfo i in info)
        {
            if(File.Exists(fileName))
            {
                fileExists = true;
                print("IT'S A MATCH");
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
                    writer.WriteLine(columns);
                    writer.WriteLine(rows);
                    print(getTilemapInformation(columns, rows));
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
