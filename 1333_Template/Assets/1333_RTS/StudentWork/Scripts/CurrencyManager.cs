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

        UpdateText(Army.Player);
        UpdateText(Army.Enemy);

        //currentGold = startingGold;
        //UpdateText();
    }

    public int GetGold(Army army)
    {
        return currentGoldByArmy.TryGetValue(army, out int gold) ? gold : 0;
    }

    public bool CanAfford(Army army, Unit Unit)
    {
        return GetGold(army) >= Unit.Cost;
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
            UpdateText(army);
        }
    }

    public bool TryBuyUnit(Army army, Unit Unit)
    {
        if (CanAfford(army, Unit))
        {
            currentGoldByArmy[army] -= Unit.Cost;
            UpdateText(army);
            return true;
        }
        return false;
    }

    private void UpdateText(Army army)
    {
        if (army == Army.Player)
        {
            UIManager.Instance.GoldText.text = currentGoldByArmy[army].ToString() + " Gold";
        }
        else if (army == Army.Enemy)
        {
            Debug.Log($"Enemy Gold: {currentGoldByArmy[army]}");
        }
    }
}
