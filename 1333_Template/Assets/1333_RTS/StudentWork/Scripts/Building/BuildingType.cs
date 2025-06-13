using System.Collections;
using System.Collections.Generic;
using RTS_1333;
using UnityEngine;
[CreateAssetMenu(fileName = "BuildingType", menuName = "Game/BuildingType")]
public class BuildingType : UnitType
{

    [SerializeField] private int cost = 1;
    [SerializeField] private int value = 1;

    public GameObject unitPrefab;

    public int Cost => cost;
    public int Value => value;

}