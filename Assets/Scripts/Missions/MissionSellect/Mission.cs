using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "mission")]
public class Mission : ScriptableObject
{
    public Sprite missionPreview;
    public List<string> charactersRequired;
    public string missionDescription;
    public string missionName;
    public int sceneIndex;
}
