using RTS_1333;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    [SerializeField] private int startingGold;

    private Dictionary<Army, int> currentGoldByArmy = new();
    //public int CurrentGold => currentGold; 

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        currentGoldByArmy[Army.Player] = startingGold;
        currentGoldByArmy[Army.Enemy] = startingGold;

        UpdateText();

        //currentGold = startingGold;
        //UpdateText();
    }

    public int GetGold(Army army)
    {
        return currentGoldByArmy.TryGetValue(army, out int gold) ? gold : 0;
    }

    public bool CanAfford(Army army, Unit unit)
    {
        return GetGold(army) >= unit.Cost;
    }

    public bool CanAfford(Army army, int amt)
    {
        return GetGold(army) >= amt;
    }

    public void EarnGold(Army army, int amount = 1)
    {
        if (currentGoldByArmy.ContainsKey(army))
        {
            currentGoldByArmy[army] += amount;
            UpdateText();
        }
    }

    public bool TryBuyUnit(Army army, Unit unit)
    {
        if (CanAfford(army, unit))
        {
            currentGoldByArmy[army] -= unit.Cost;
            UpdateText();
            return true;
        }
        return false;
    }

    private void UpdateText()
    {
        /*if (army == Army.Player)
        {
            UIManager.Instance.GoldText.text = currentGoldByArmy[army].ToString() + " Gold";
        }
        else if (army == Army.Enemy)
        {
            Debug.Log($"Enemy Gold: {currentGoldByArmy[army]}");
        }*/

        UIManager.Instance.GoldText.text = $"Your Gold: {currentGoldByArmy[Army.Player].ToString()} \n Enemy Gold: {currentGoldByArmy[Army.Enemy].ToString()}";
    }
}
