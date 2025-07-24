using System.Collections;
using System.Collections.Generic;
using RTS_1333;
using UnityEngine;
[CreateAssetMenu(fileName = "BuildingType", menuName = "Game/BuildingType")]
public class BuildingType : UnitType
{
    [SerializeField] private int value = 1;
    [SerializeField] private int spawnInterval = 1;
    [SerializeField] private int buildTime = 1;
    public int Value => value;
    public int SpawnInterval => spawnInterval;
    public int BuildTime => buildTime;

}