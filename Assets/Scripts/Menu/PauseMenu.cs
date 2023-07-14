using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GraphicRaycaster))]
public class PauseMenu : Menu
{
    // Pause the game when the pause menu is enabled
    public void OnEnable() => GameManager.Instance.isPaused = true;
    
    // Unpause the game when the pause menu is disabled
    public void OnDisable() => GameManager.Instance.isPaused = false;
}
