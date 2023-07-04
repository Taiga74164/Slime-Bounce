using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    [Header("Camera Components")]
    public float cameraSpeed = 5.0f;

    [Header("UI Components")]
    //

    private float _score = 0;
    private Transform playerTransform;
    private bool _isGameOver = false;

    private void Start()
    {
        playerTransform = FindObjectOfType<PlayerController>().transform;
    }

    private void LateUpdate()
    {
        if (_isGameOver || playerTransform == null)
            return;
        
        if (playerTransform.position.y > transform.position.y)
            transform.position = Vector3.Lerp(transform.position,
                new Vector3(transform.position.x, playerTransform.position.y, transform.position.z),
                cameraSpeed * Time.deltaTime);
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Player"))
            return;

        _isGameOver = true;
        
        RestartScene();
        // TODO:
        // 1. Add code to show UI
        // 2. Use PlayerPrefs to keep track of best score
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    public void RestartScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}
