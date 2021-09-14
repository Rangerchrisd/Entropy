using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

using UnityEngine.EventSystems;

public class DoorTile : MonoBehaviour
{
    public List<Hex> myHexes;
    public static event Action<DoorTile, Hex> doorOpened;
    public Hex singleHexDoor;
    public GameObject nextArea;
    public bool opened,BigTile, cantOpen;
    // Start is called before the first frame update
    public void Awake()
    {
        if (myHexes.Count != 0)
        {
            if (!BigTile)
                Debug.Log(gameObject.name + ":it looks like you wanted a bigtile so we made it a big tile if this was a mistake fix it");
            BigTile = true;
            if (BigTile)
                PrimaryCharacter.onMoveSquare += PrimaryCharacter_onMoveSquare;
        }
        else {
            if (!singleHexDoor) {
                Hex fallbackPlan;
                if (this.gameObject.TryGetComponent<Hex>(out fallbackPlan))
                {
                    Debug.Log(gameObject.name + ":Hey, you messed up and forgot to add a tile for this door to activate on, luckily fallbackplan worked, because you put it on the hex");
                    singleHexDoor = fallbackPlan;
                }
                else
                {
                    Debug.Log(gameObject.name + ":Hey, you messed up and forgot to add a tile for this door to activate on.");
                }
            }
        }
    }
    public void OnDestroy()
    {
        if (BigTile)
            PrimaryCharacter.onMoveSquare -= PrimaryCharacter_onMoveSquare;
    }
    private void Update()
    {
        if (cantOpen)
            return;
        if (!BigTile) {
            if (singleHexDoor.myUnit)
            {
                if (singleHexDoor.myUnit is PrimaryCharacter)
                {
                    openDoor(singleHexDoor);
                }
            }
        }
    }
    private void PrimaryCharacter_onMoveSquare(Unit obj)
    {
        if (!opened&&!cantOpen) {
            if (myHexes.Contains(obj.myHex)) {
                openDoor(obj.myHex);
            }
        }
    }
    public void openDoor(Hex hex) {
        if (!nextArea.activeInHierarchy)
        {
            nextArea.SetActive(true);
        }
        opened = true;
        doorOpened?.Invoke(this, hex);

    }
}
