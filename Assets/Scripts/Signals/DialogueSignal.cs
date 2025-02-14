using System;
using UnityEngine;

public class DialogueSignal : ISignal
{
    public string Key;
    public DialogueSignal(string key)
    {
        Key = key;
    }
}
