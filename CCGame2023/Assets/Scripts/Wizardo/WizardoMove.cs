using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class WizardoMove : MonoBehaviour
{
    // Start is called before the first frame update
    public int teleportRange;
    int tempRange;
    Rigidbody2D rb;
    [SerializeField] Tilemap tilemap;
    [SerializeField] float teleportTimerLength;
    
    
    float teleportTimer;
    
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        teleport();
        teleportTimer = teleportTimerLength;
    }

    // Update is called once per frame
    void Update()
    {
        teleportTimer -= Time.deltaTime;
        if(teleportTimer < 0f)
        {
            teleport();
            teleportTimer = teleportTimerLength;
        }
    }

    public void teleport()
    {
        int counter = 0;
        int random = Mathf.FloorToInt(Random.Range(0,500));
        bool teleported = false;
        List<Vector3> potentialTeleportLocations = new List<Vector3>();
        

        tempRange = teleportRange;
        bool noTiles = true;

        while(potentialTeleportLocations.Count < 1)
        {
            for(int row = 0; row < tempRange; row++)
            {
                for(int col = 0; col < tempRange; col++)
                {
                    Vector3Int tilePosition = new Vector3Int(Mathf.FloorToInt(transform.position.x+ 0.5f -(tempRange*0.5f)+col), Mathf.FloorToInt(transform.position.y + 0.5f -(tempRange*0.5f)+row), 0);
                    if(tilemap.GetTile(tilePosition) != null && tilemap.GetTile(tilePosition + new Vector3Int(0, 1, 0)) == null && tilemap.GetTile(tilePosition + new Vector3Int(0, 2, 0)) == null && tilemap.GetTile(tilePosition + new Vector3Int(0, 3, 0)) == null && (tilemap.GetTile(tilePosition + new Vector3Int(-1, 3, 0)) == null || tilemap.GetTile(tilePosition + new Vector3Int(1, 3, 0)) == null))
                    {
                        RaycastHit2D hit = Physics2D.BoxCast(new Vector2(tilePosition.x, tilePosition.y + 3.5f), new Vector2(1f, 1f), 0f, -transform.up, 1.9f);
                        if(hit.collider == null)
                        {
                            potentialTeleportLocations.Add(tilePosition + new Vector3(0.5f, 2.5f, 0f));
                        }
                    }
                }
            }
            if(potentialTeleportLocations.Count < 1)
            {
                tempRange++;
            }
            if(tempRange>100)
            {
                Destroy(gameObject);
            }
            
        }
        transform.position = potentialTeleportLocations[(random % potentialTeleportLocations.Count)];
        rb.velocity = new Vector3(0f, 0f, 0f);
            
    }
}
