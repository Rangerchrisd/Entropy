using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class objectIDforGameManagement : MonoBehaviour
{
    public enum objectID
    {

        hex = 0,
        wall = 1,
        unwalkableHex = 2
    }
    public int myID;
    public objectID myObjectID;
    public void Update()
    {

    }
    //hex 0
    //wall 1
    //unwalkable hex 2
    //this way I can update prefabs that are no longer prefabs.
}
