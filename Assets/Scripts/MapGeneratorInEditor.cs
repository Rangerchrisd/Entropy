using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

[ExecuteInEditMode]
public class MapGeneratorInEditor : MonoBehaviour
{
    List<Hex> hexCollection = new List<Hex>();
    public Tilemap map;
    public GameObject[] HexFloors;
    public static event Action HexMapFinished;
    public bool DoThing;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Update()
    {
        if (DoThing)
        {

            DoThing = false;
            Hex[] hexesThatWereThere = FindObjectsOfType<Hex>();
            foreach (Hex i in hexesThatWereThere)
            {
                hexCollection.Add(i);
            }
            foreach (var pos in map.cellBounds.allPositionsWithin)
            {
                Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
                Vector3 place = map.CellToWorld(localPlace);
                if (!map.HasTile(pos))
                {
                    continue;
                }else if (map.GetTile(localPlace).name == "HexagonBasic")
                {
                    //GameObject adding = Instantiate(HexFloors[0], place, HexFloors[0].gameObject.transform.rotation);
                    GameObject adding = Instantiate(HexFloors[0], place, HexFloors[0].gameObject.transform.rotation);
                    hexCollection.Add(adding.GetComponent<Hex>());
                    adding.transform.parent = this.transform;
                    adding.name = "tile:x:" + place.x + "y:" + place.y + "z:" + place.z;
                    //Destroy(map.GetTile(localPlace));
                    map.SetTile(localPlace, null);
                }else if (map.GetTile(localPlace).name == "WallTile")
                {
                    //GameObject adding = Instantiate(HexFloors[0], place, HexFloors[0].gameObject.transform.rotation);
                    GameObject adding = Instantiate(HexFloors[1], place, HexFloors[1].gameObject.transform.rotation);
                    hexCollection.Add(adding.GetComponent<Hex>());
                    adding.transform.parent = this.transform;
                    adding.name = "Wall:x:" + place.x + "y:" + place.y + "z:" + place.z;
                    //Destroy(map.GetTile(localPlace));
                    map.SetTile(localPlace, null);
                }else if (map.GetTile(localPlace).name == "NotWalkable")
                {
                    //GameObject adding = Instantiate(HexFloors[0], place, HexFloors[0].gameObject.transform.rotation);
                    GameObject adding = Instantiate(HexFloors[2], place, HexFloors[2].gameObject.transform.rotation);
                    //hexCollection.Add(adding.GetComponent<Hex>());
                    adding.transform.parent = this.transform;
                    adding.name = "NotGround:x:" + place.x + "y:" + place.y + "z:" + place.z;
                    map.SetTile(localPlace, null);
                }
            }
            foreach (Hex i in hexCollection)
            {
                i.HexMapMade();
            }
            //map.gameObject.SetActive(false);
            HexMapFinished?.Invoke();
        }
    }
}
