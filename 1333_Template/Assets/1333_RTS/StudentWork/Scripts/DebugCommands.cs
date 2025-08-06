using System;
using System.Collections;
using System.Collections.Generic;
using IngameDebugConsole;
using RTS_1333;
using UnityEngine;

public class DebugCommands : MonoBehaviour
{
    private void OnEnable()
    {
        DebugLogConsole.AddCommand<int>("HelloWorld", "Prints a message to the console", HelloWorld);
    }

    private void HelloWorld(int obj)
    {
        //Debug.Log("Hello world!");
    }

    private void OnDisable()
    {
        DebugLogConsole.RemoveCommand("HelloWorld");

    }
}
