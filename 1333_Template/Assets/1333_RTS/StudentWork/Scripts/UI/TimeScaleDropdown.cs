using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using RTS_1333;
using TMPro;

public class TimeStateDropdown : MonoBehaviour
{
    public TMP_Dropdown dropdown;

    void Start()
    {
        dropdown.ClearOptions();

        List<string> options = new List<string>();
        foreach (TimeState state in Enum.GetValues(typeof(TimeState)))
        {
            options.Add(FormatTimeState(state));
        }

        dropdown.AddOptions(options);

        dropdown.onValueChanged.AddListener(OnDropdownChanged);
    }

    public void OnDropdownChanged(int index)
    {
        TimeState selectedState = (TimeState)Enum.GetValues(typeof(TimeState)).GetValue(index);
        Debug.Log("Selected TimeState: " + selectedState);

        GameManager.Instance.SetTimeState(selectedState);
    }

    string FormatTimeState(TimeState state)
    {
        return $"{state} ({(int)state}x)";
    }

    public void ForceState(TimeState state)
    {
        dropdown.value = (int)state;
        OnDropdownChanged((int)state);
    }
}
