using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GraphicRaycaster))]
public class PauseMenu : Menu
{
    // Pause the game when the pause menu is enabled
    public void OnEnable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.isPaused = true;
        if (AudioManager.Instance != null)
            AudioManager.Instance.ost.volume /= 2;
    }
    
    // Unpause the game when the pause menu is disabled
    public void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.isPaused = false;
        if (AudioManager.Instance != null)
            AudioManager.Instance.ost.volume *= 2;
    }
}
