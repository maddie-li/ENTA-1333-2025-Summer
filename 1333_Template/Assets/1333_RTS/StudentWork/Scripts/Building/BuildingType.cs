using System.Collections;
using System.Collections.Generic;
using RTS_1333;
using UnityEngine;
[CreateAssetMenu(fileName = "BuildingType", menuName = "Game/BuildingType")]
public class BuildingType : ScriptableObject
{
    [SerializeField] private string buildingName;

    [SerializeField] private int width = 1;
    [SerializeField] private int length = 1;

    [SerializeField] private int maxHP = 1;
    [SerializeField] private int cost = 1;
    [SerializeField] private int value = 1;

    public GameObject unitPrefab;

    public int Width => width;
    public int Length => length;

    public int MaxHP => maxHP;
    public int Cost => cost;
    public int Value => value;

}