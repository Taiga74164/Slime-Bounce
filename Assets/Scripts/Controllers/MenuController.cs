using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject playButton;

    private bool playerConnected = false;

    public void OnPlayerConnected()
    {
        playerConnected = true;
    }

    public void Play()
    {
        SceneManager.LoadScene("Game");
    }
}
