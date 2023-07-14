using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;


public class GameManager : Singleton<GameManager>
{
    public GameObject padPrefab;
    public GameObject superPadPrefab;
    public int numberOfPadsToMake = 10;
    public float minVerticalDistance;
    public float maxVerticalDistance;

    public PauseMenu pausePanel; // UI Panel that will show when game is paused
    public bool isPaused = false; // Track if the game is paused

    private List<GameObject> _pads = new List<GameObject>();
    private Vector2 _spawnPosition;
    private int _indexToCheck = 5;
    private int _indexToTranslate = 0;
    private bool _superCharge = false;
    private float _levelWidth;

    private void Start()
    {
        _spawnPosition = transform.position;
        _levelWidth = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).x -
                     padPrefab.GetComponent<SpriteRenderer>().bounds.extents.x / 2f;
        CreatePads();
    }
    
    private void Update()
    {
        // Check if the "Esc" key was pressed
        if (Input.GetKeyUp(KeyCode.Escape) && !pausePanel.IsOpen && !isPaused)
            PauseGame();


        if (_indexToCheck < _pads.Count)
        {
            if (transform.position.y >= _pads[_indexToCheck].transform.position.y)
                TranslatePad(_indexToTranslate);
        }
    }
    
    private void CreatePads()
    {
        for (int i = 0; i < numberOfPadsToMake; i++)
            CreatePad();
    }

    private void CreatePad()
    {
        _spawnPosition = new Vector2(0f, _spawnPosition.y);
        _spawnPosition += new Vector2(Random.Range(-_levelWidth, _levelWidth), Random.Range(minVerticalDistance, maxVerticalDistance));



        GameObject padTemp;
        if (!_superCharge && Random.Range(0, 3) == 0)
        {
            _superCharge = true;
            padTemp = Instantiate(superPadPrefab, _spawnPosition, Quaternion.identity);
        }
        else
        {
            padTemp = Instantiate(padPrefab, _spawnPosition, Quaternion.identity);
        }



        _pads.Add(padTemp);
    }
    
    private void TranslatePad(int padIndex)
    {
        GameObject padToTranslate = _pads[padIndex];
        padToTranslate.transform.position = new Vector2(0f, padToTranslate.transform.position.y);



        _spawnPosition = new Vector2(0f, _spawnPosition.y);
        _spawnPosition += new Vector2(Random.Range(-_levelWidth, _levelWidth), Random.Range(minVerticalDistance, maxVerticalDistance));



        padToTranslate.transform.position = _spawnPosition;



        StartCoroutine(GrowPad(padToTranslate));



        if (_indexToTranslate < _pads.Count - 1)
        {
            _indexToTranslate++;
        }
        else
        {
            _indexToTranslate = 0;
        }



        _indexToCheck = (_indexToTranslate + 5) % (_pads.Count - 1);
    }
    
    private IEnumerator GrowPad(GameObject padObject)
    {
        for (int i = 0; i <= 10; i++)
        {
            float scaleValue = (float)i / 10;
            padObject.transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void PauseGame() => pausePanel.OpenMenu();

    public void ResumeGame() => pausePanel.CloseMenu();

    // Restart Button 
    public void RestartGame() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    // Main Menu Button
    public void MainMenu() => SceneManager.LoadScene("Menu");
}