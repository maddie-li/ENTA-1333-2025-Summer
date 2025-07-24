using RTS_1333;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    [SerializeField] private int startingGold;

    private int currentGold;
    public int CurrentGold => currentGold; 

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        currentGold = startingGold;
        UpdateText();
    }

    public bool CanAfford(Unit unit)
    {
        return currentGold >= unit.Cost;
    }

    public void EarnGold()
    {
        currentGold += 1;
        UpdateText();
    }

    public void EarnGold(int amout)
    {
        currentGold += amout;
        UpdateText();
    }

    public bool TryBuyUnit(Unit unit)
    {
        if (CanAfford(unit))
        {
            currentGold -= unit.Cost;
            UpdateText();
            return true;
        }
        return false;
    }

    private void UpdateText()
    {
        UIManager.Instance.GoldText.text = currentGold.ToString() + " Gold";
    }
}
