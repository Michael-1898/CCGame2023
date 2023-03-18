using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class TurnArrayToStatic : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] List<Tile> allTiles = new List<Tile>();
    [SerializeField] List<string> allTileCharacters = new List<string>();
    [SerializeField] List<GameObject> allTileGameObjects = new List<GameObject>();
    [SerializeField] List<Vector3> allTileSizes = new List<Vector3>();
    [SerializeField] List<int> allTileLimits = new List<int>();



    void Start()
    {
        tilemap.allTiles = allTiles;
        tilemap.allTileCharacters = allTileCharacters;
        tilemap.allTileGameObjects = allTileGameObjects;
        tilemap.allTileSizes = allTileSizes;
        tilemap.allTileLimits = allTileLimits;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
