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
    public List<Button> allTileButtons = new List<Button>();
    [SerializeField] private Text levelNameText;
    [SerializeField] private string levelInformation;
    public GameObject tilePreview;
    SpriteRenderer tilePreviewSR;
    Tile currentTile;
    

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
            tilePreviewSR.sprite = currentTile.sprite;
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            tilePreview.transform.position = new Vector3(Mathf.Floor(mousePos.x)+0.5f, Mathf.Floor(mousePos.y)+0.5f, 0f);
            if(Input.GetKeyDown("escape"))
            {
                print("escape");
                currentTile = null;
                tilePreviewSR.sprite = null;
            }
            if(Input.GetMouseButton(0))
            {
                tilemap1.SetTile(new Vector3Int(Mathf.FloorToInt(tilePreview.transform.position.x - 0.5f), Mathf.FloorToInt(tilePreview.transform.position.y - 0.5f), 0), currentTile);
            }
        }
        else tilePreviewSR.sprite = null;

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
                if(levelInformation.Substring(k, 1) == allTileCharacters[l])
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
            }
        }
        print(currentTile);
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
