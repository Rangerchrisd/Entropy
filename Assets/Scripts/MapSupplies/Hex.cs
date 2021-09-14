using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class Hex : MonoBehaviour, IPointerDownHandler
{
    public bool isObjective;
    public List<Hex> Path = new List<Hex>();
    public Hex[] adjacentHexes = new Hex[6];
    public Hex distanceFrom;
    public Hex alternate;
    public Color originalColor, avalibleSpaceColor, enemySpaceColor, objectiveColor;
    public int distanceTo;
    public Renderer myRender;
    public GameObject highlightArrowPrefab;
    public GameObject highlightArrowGameobject;
    public Unit myUnit;
    public Interactable myInteractable;
    public static event Action<Hex> onClick;
    public static event Action<Hex> spawnHex;
    public static event Action<Hex> removeHex;

    public void Start() {
        myRender.material.color = originalColor;
        spawnHex?.Invoke(this);
        PlayerTurn.resetPathing += resetMax;
        Enemy.resetPathing += resetMax;
    } 
    public void OnDestroy()
    {
        removeHex?.Invoke(this);
        PlayerTurn.resetPathing -= resetMax;
        Enemy.resetPathing -= resetMax;
    }
    public void enterHex(Unit character) {
        HexProperty[] thingsToDo = this.GetComponents<HexProperty>();
        foreach (HexProperty i in thingsToDo)
            i.onEnter(character);
    }
    public void findDistance()
    {
        Path.Clear();
        findDistance(0);
    }
    public void findDistance(int lowest)
    {
        distanceTo = lowest;
        Path.Add(this);
        foreach (Hex i in adjacentHexes)
        {
            if (i != null)
            {
                if (i.distanceTo > distanceTo + 1)
                {
                    if (i.distanceTo != 0)
                    {
                        i.Path = copyHexPath();
                        i.findDistance(distanceTo + 1);
                    }
                }
            }
        }

        if (alternate != null)
        {
            if (alternate.distanceTo != 999 || alternate.distanceTo > distanceTo + 1)
            {
                alternate.Path = copyHexPath();
                alternate.findDistance(distanceTo + 1);
            }
        }
    }
    public void playerFindDistance()
    {
        Path.Clear();
        playerFindDistance(0);
    }
    public void playerFindDistance(int lowest)
    {
        distanceTo = lowest;
        Path.Add(this);
        foreach (Hex i in adjacentHexes)
        {
            if (i != null)
            {
                if (i.distanceTo > distanceTo + 1)
                {
                    if (i.distanceTo != 0)
                    {
                        if (myUnit)
                        {
                            if (myUnit.GetType() == typeof(Enemy))
                            {

                            }
                            else if (myUnit.GetType() == typeof(PrimaryCharacter))
                            {
                                i.Path = copyHexPath();
                                i.playerFindDistance(distanceTo + 1);

                            }
                            else if (myUnit.GetType() == typeof(SecondaryCharacter))
                            {
                                i.Path = copyHexPath();
                                i.playerFindDistance(distanceTo + 1);

                            }
                            else
                            {

                            }
                        }
                        else
                        {
                            i.Path = copyHexPath();
                            i.playerFindDistance(distanceTo + 1);
                        }
                    }
                }
            }
        }
        if (alternate != null)
        {
            if (alternate.distanceTo > distanceTo + 1)
            {
                alternate.Path = copyHexPath();
                alternate.playerFindDistance(distanceTo + 1);
            }
        }
    }


    public List<Character> findCharacters() {
        return new List<Character>();
    }

    public void resetMax()
    {
        distanceTo = 999;
    }

    public List<Hex> copyHexPath()
    {
        List<Hex> returner = new List<Hex>();
        foreach(Hex i in Path)
            returner.Add(i);
        return returner;
    }
    public void changeSingleColor(Color myColor) {
        if(myRender)
            myRender.material.color = myColor;
    }

    public void changeColor(int MaxRange,int type)
    {
        if ((int)TargetTypes.EmptyHex == type)
        {
            if (distanceTo <= MaxRange)
            {
                if (!myUnit)
                {
                    myRender.material.color = avalibleSpaceColor;
                    return;
                }
            }
        }
        if ((int)TargetTypes.Enemy == type)
        {
            if (distanceTo <= MaxRange)
            {
                if (myUnit)
                {
                    Debug.Log("3");
                    if (myUnit.GetType() == typeof(Enemy))
                    {
                        myRender.material.color = enemySpaceColor;
                        return;
                    }
                }
            }
        }
        if ((int)TargetTypes.Hex == type)
        {
            if (distanceTo <= MaxRange)
            {
                myRender.material.color = avalibleSpaceColor;
                return;
            }
        }
        if ((int)TargetTypes.Noncombat == type)
        {
            myRender.material.color = avalibleSpaceColor;
            return;
        }
        myRender.material.color = originalColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        onClick?.Invoke(this);
    }
    public void HexMapMade()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, new Vector3(0.5f,0, 0.5f), out hit);
        if (hit.collider != null)
        {
            //Top Right
            if (hit.collider.gameObject.tag == "Hex" && Vector3.Distance(transform.position, hit.collider.transform.position) <= 1)
                adjacentHexes[0] = hit.collider.gameObject.GetComponent<Hex>();
        }
        Physics.Raycast(transform.position, new Vector3(1, 0, 0), out hit);
        if (hit.collider != null)
        {
            //Right
            if (hit.collider.gameObject.tag == "Hex" && Vector3.Distance(transform.position, hit.collider.transform.position) <= 1)
                adjacentHexes[1] = hit.collider.gameObject.GetComponent<Hex>();
        }
        Physics.Raycast(transform.position, new Vector3(0.5f, 0, -0.5f), out hit);
        if (hit.collider != null)
        {
            //Bottom Right
            if (hit.collider.gameObject.tag == "Hex" && Vector3.Distance(transform.position, hit.collider.transform.position) < 1)
                adjacentHexes[2] = hit.collider.gameObject.GetComponent<Hex>();
        }
        Physics.Raycast(transform.position, new Vector3(-0.5f, 0, -0.5f), out hit);
        if (hit.collider != null)
        {
            //Bottom Left
            if (hit.collider.gameObject.tag == "Hex" && Vector3.Distance(transform.position, hit.collider.transform.position) <= 1)
                adjacentHexes[3] = hit.collider.gameObject.GetComponent<Hex>();
        }
        Physics.Raycast(transform.position, new Vector3(-1, 0, 0), out hit);
        if (hit.collider != null)
        {
            //Left
            if (hit.collider.gameObject.tag == "Hex" && Vector3.Distance(transform.position, hit.collider.transform.position) <= 1)
                adjacentHexes[4] = hit.collider.gameObject.GetComponent<Hex>();
        }
        Physics.Raycast(transform.position, new Vector3(-0.5f, 0, 0.5f), out hit);
        if (hit.collider != null)
        {
            //Top Left
            if (hit.collider.gameObject.tag == "Hex" && Vector3.Distance(transform.position, hit.collider.transform.position) <= 1)
                adjacentHexes[5] = hit.collider.gameObject.GetComponent<Hex>();
        }
    }

    public void highlightHex() {
        highlightArrowGameobject= 
        Instantiate(highlightArrowPrefab, this.gameObject.transform);
    }

    public void unhighlightHex() {
        Destroy(highlightArrowGameobject);
    }
    public Hex randomAdjacentEmptyHex() {
        List<Hex> possibleHexes = new List<Hex>();
        foreach (Hex i in adjacentHexes) {
            if (i) {
                if (!i.myInteractable && !i.myUnit) {
                    possibleHexes.Add(i);
                }
            }
        }
        return possibleHexes[UnityEngine.Random.Range(0, possibleHexes.Count)]; ;
    }
}
