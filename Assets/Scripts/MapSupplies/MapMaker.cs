﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class MapMaker : MonoBehaviour
{
    List<Hex> hexCollection = new List<Hex>();
    public Tilemap map;
    public GameObject[] HexFloors;
    public static event Action HexMapFinished;
    // Start is called before the first frame update
    void Start()
    {
        PlayerTurn playerTurn = FindObjectOfType<PlayerTurn>();
        Hex[] hexesThatWereThere = FindObjectsOfType<Hex>();
        foreach (Hex i in hexesThatWereThere)
        {
            hexCollection.Add(i);
            playerTurn.gameManager.combatManager.allHexes.Add(i);
        }
        foreach (var pos in map.cellBounds.allPositionsWithin)
        {
            Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
            Vector3 place = map.CellToWorld(localPlace);
            if (!map.HasTile(pos))
            {
                continue;
            }
            if (map.GetTile(localPlace).name == "HexagonBasic")
            {
                GameObject adding = Instantiate(HexFloors[0], place, HexFloors[0].gameObject.transform.rotation);
                hexCollection.Add(adding.GetComponent<Hex>());
                adding.transform.parent = this.transform;
                adding.name = "tile:x:" + place.x + "y:" + place.y + "z:" + place.z;
            }
        }
        foreach (Hex i in hexCollection)
        {
            i.HexMapMade();
        }
        map.gameObject.SetActive(false);
        HexMapFinished?.Invoke();
    }
}
