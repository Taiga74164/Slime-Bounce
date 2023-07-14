using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Menu<T> : Singleton<T> where T : Menu<T>
{
    public virtual void Open() => gameObject.SetActive(true);
    
    public virtual void Close() => gameObject.SetActive(false);
}

[RequireComponent(typeof(Canvas))]
public abstract class Menu : Menu<Menu>
{
    protected virtual void Update()
    {
        if (Input.GetButtonUp("Cancel") && IsOpen)
            CloseMenu();
    }
    
    public bool IsOpen => MenuManager.Instance.IsMenuOpen(this);
    
    public virtual void OpenMenu() => MenuManager.Instance.OpenMenu(this);
    
    public virtual void CloseMenu() => MenuManager.Instance.CloseMenu();
}