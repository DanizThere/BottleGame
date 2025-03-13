using System;
using UnityEngine;

public abstract class UIScreen
{
    public event Action OnEnabled;
    public event Action OnDisabled;
    public GameObject Menu;

    public UIScreen()
    {
        OnEnabled += OnEnable;
        OnDisabled += OnDisable;
    }

    public abstract void OnEnable();
    public abstract void OnDisable();
}
