﻿using System.Collections;
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
    [SerializeField] Tilemap tilemap;
    
    void Start()
    {
        teleport();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void teleport()
    {
        int counter = 0;
        int random = Mathf.FloorToInt(Random.Range(0,100));
        bool teleported = false;
        
        print(random);
        while(teleported == false)
        {
            for(int row = 0; row < teleportRange; row++)
            {
                for(int col = 0; col < teleportRange; col++)
                {
                    Vector3Int tilePosition = new Vector3Int(Mathf.FloorToInt(transform.position.x+ 0.5f -(teleportRange*0.5f)+col), Mathf.FloorToInt(transform.position.y + 0.5f -(teleportRange*0.5f)+row), 0);
                    if(tilemap.GetTile(tilePosition) != null)
                    {
                        if(counter == random)
                        {
                            transform.position = tilePosition + new Vector3(1f, 3f, 0f);
                            teleported = true;
                        }
                        counter++;
                    }
                }
            }
        }
            
    }
}
