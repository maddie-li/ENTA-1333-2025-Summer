using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "BuildingType", menuName = "Game/BuildingType")]
public class BuildingType : ScriptableObject
{
    public List<BuildingData> buildings = new();
}

[System.Serializable]
public class BuildingData
{
    public string BuildingName;

}