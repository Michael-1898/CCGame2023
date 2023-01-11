using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class TurnArrayToStatic : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] List<Tile> allTiles = new List<Tile>();
    [SerializeField] List<string> allTileCharacters = new List<string>();
    
    void Start()
    {
        tilemap.allTiles = allTiles;
        tilemap.allTileCharacters = allTileCharacters;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
