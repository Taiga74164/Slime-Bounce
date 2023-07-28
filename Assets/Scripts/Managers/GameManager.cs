using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{
    [Header("Platform Prefabs")]
    public PlatformController platformLeftPrefab;
    public PlatformController platformFrontPrefab;
    public PlatformController platformRightPrefab;

    [Header("Platform Spawn Points")]
    public Transform rowTransform1;
    public Transform rowTransform2;
    public Transform rowTransform3;
    
    private int _randomIndex;
    public int numberOfPadsToMake = 10;
    public float minVerticalDistance;
    public float maxVerticalDistance;

    [Header("Pause Menu")]
    public PauseMenu pausePanel; // UI Panel that will show when the game is paused
    public bool isPaused = false; // Track if the game is paused

    private List<PlatformController> _pads = new List<PlatformController>();
    private List<Vector3> _occupiedSpawnPoints = new List<Vector3>();
    private Vector2 _spawnPosition;
    private int _indexToCheck = 5;
    private int _indexToTranslate = 0;
    private float _levelWidth;

    [Header("Spawn Rates")]
    public float normalSpawnRate = 0.6f;
    public float trapSpawnRate = 0.2f;
    public float boostSpawnRate = 0.2f;

    private void Start()
    {
        _spawnPosition = transform.position;
        _levelWidth = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).x -
                      platformFrontPrefab.GetComponent<SpriteRenderer>().bounds.extents.x / 2f;
        
        CreatePads();
    }

    private void Update()
    {
        // Check if the "Esc" key was pressed
        if (Input.GetButtonUp("Cancel") && !pausePanel.IsOpen && !isPaused)
            PauseGame();

        if (_indexToCheck < _pads.Count)
        {
            if (transform.position.y >= _pads[_indexToCheck].transform.position.y)
                TranslatePad(_indexToTranslate);
        }
    }

    private void CreatePads()
    {
        // Dictionary to store available spawn points for each row
        var availableSpawnPoints = new Dictionary<Transform, List<Transform>>();
        availableSpawnPoints[rowTransform1] = new List<Transform> { rowTransform1.Find("Point1"), rowTransform1.Find("Point2"), rowTransform1.Find("Point3") };
        availableSpawnPoints[rowTransform2] = new List<Transform> { rowTransform2.Find("Point1"), rowTransform2.Find("Point2"), rowTransform2.Find("Point3") };
        availableSpawnPoints[rowTransform3] = new List<Transform> { rowTransform3.Find("Point1"), rowTransform3.Find("Point2"), rowTransform3.Find("Point3") };
    
        for (var i = 0; i < numberOfPadsToMake; i++)
        {
            // Choose a random row index (0, 1, or 2) to determine the row to spawn the platform
            var randomRowIndex = Random.Range(0, 3);
            var rowTransform = randomRowIndex == 0 ? rowTransform1 : (randomRowIndex == 1 ? rowTransform2 : rowTransform3);
    
            // Choose a random number to determine the platform type
            var randomValue = Random.value;
            PlatformController platformPrefab;
    
            if (randomValue < normalSpawnRate)
            {
                platformPrefab = GetPlatformPrefabForType(PlatformType.Normal);
                platformPrefab.platformType = PlatformType.Normal;
            }
            else if (randomValue < normalSpawnRate + trapSpawnRate)
            {
                platformPrefab = GetPlatformPrefabForType(PlatformType.Trap);
                platformPrefab.platformType = PlatformType.Trap;
            }
            else
            {
                platformPrefab = GetPlatformPrefabForType(PlatformType.Boost);
                platformPrefab.platformType = PlatformType.Boost;
            }
    
            // Get the list of available spawn points for the chosen row
            var availablePointsForRow = availableSpawnPoints[rowTransform];
    
            // Ensure that there are available spawn points for this row
            if (availablePointsForRow.Count == 0)
            {
                // No available spawn points left for this row, break out of the loop
                Debug.Log($"No available spawn points left for row {randomRowIndex + 1}.");
                break;
            }
    
            // Choose a random spawn point from the available points list
            var randomSpawnPointIndex = Random.Range(0, availablePointsForRow.Count);
            var spawnPoint = availablePointsForRow[randomSpawnPointIndex].position;

            // Remove the used spawn point from the available points list
            availablePointsForRow.RemoveAt(randomSpawnPointIndex);

            // Add the used spawn point to the occupied list
            _occupiedSpawnPoints.Add(spawnPoint);

            // Instantiate the platform at the chosen spawn point
            var padTemp = Instantiate(platformPrefab, spawnPoint, Quaternion.identity);
            padTemp.transform.position = new Vector3(padTemp.transform.position.x, padTemp.transform.position.y, 0f);
            _pads.Add(padTemp);
        }
    }

    private PlatformController GetPlatformPrefabForType(PlatformType platformType)
    {
        switch (platformType)
        {
            case PlatformType.Normal:
                return platformFrontPrefab;
            case PlatformType.Boost:
                return platformLeftPrefab;
            case PlatformType.Trap:
                return platformRightPrefab;
            default:
                return platformFrontPrefab;
        }
    }
    
    private Transform GetRandomSpawnPoint(int rowIndex)
    {
        Transform[] spawnPointsArray;

        switch (rowIndex)
        {
            case 0:
                spawnPointsArray = new Transform[] { rowTransform1.Find("Point1"), rowTransform1.Find("Point2"), rowTransform1.Find("Point3") };
                break;
            case 1:
                spawnPointsArray = new Transform[] { rowTransform2.Find("Point1"), rowTransform2.Find("Point2"), rowTransform2.Find("Point3") };
                break;
            case 2:
                spawnPointsArray = new Transform[] { rowTransform3.Find("Point1"), rowTransform3.Find("Point2"), rowTransform3.Find("Point3") };
                break;
            default:
                spawnPointsArray = new Transform[0];
                break;
        }

        // Filter out occupied spawn points before choosing a random one
        var availableSpawnPoints = new List<Transform>(spawnPointsArray).FindAll(spawnPoint => !_occupiedSpawnPoints.Contains(spawnPoint.position));

        return availableSpawnPoints.Count > 0 ? availableSpawnPoints[Random.Range(0, availableSpawnPoints.Count)] :
            // No available spawn points left for this row, return null or a default Transform
            null;
    }

    private void TranslatePad(int padIndex)
    {
        var padToTranslate = _pads[padIndex];
        var randomRowIndex = Random.Range(0, 3);
    
        // Choose a random number to determine the platform type
        var randomValue = Random.value;
        PlatformController platformPrefab;
    
        if (randomValue < normalSpawnRate)
        {
            platformPrefab = GetPlatformPrefabForType(PlatformType.Normal);
            platformPrefab.platformType = PlatformType.Normal;
        }
        else if (randomValue < normalSpawnRate + trapSpawnRate)
        {
            platformPrefab = GetPlatformPrefabForType(PlatformType.Trap);
            platformPrefab.platformType = PlatformType.Trap;
        }
        else
        {
            platformPrefab = GetPlatformPrefabForType(PlatformType.Boost);
            platformPrefab.platformType = PlatformType.Boost;
        }
    
        // Get a random spawn point for the specified row
        var spawnPoint = GetRandomSpawnPoint(randomRowIndex);
    
        // Ensure a valid spawn point is available
        if (spawnPoint != null)
        {
            // Instantiate the platform at the specified row's spawn point
            var padTemp = Instantiate(platformPrefab, spawnPoint.position, Quaternion.identity);
    
            // Keep the same scale as the original platform
            padTemp.transform.localScale = padToTranslate.transform.localScale;
    
            padTemp.transform.position = new Vector3(padTemp.transform.position.x, padTemp.transform.position.y, 0f);
            _pads[padIndex] = padTemp;
    
            // Unmark the previous spawn point as occupied if it has a parent
            if (padToTranslate.transform.parent != null)
                _occupiedSpawnPoints.Remove(padToTranslate.transform.parent.position);
    
            StartCoroutine(GrowPad(padTemp));
    
            if (_indexToTranslate < _pads.Count - 1)
                _indexToTranslate++;
            else
                _indexToTranslate = 0;
    
            _indexToCheck = (_indexToTranslate + 5) % (_pads.Count - 1);
        }
        else
        {
            // Handle the case when no valid spawn point is available for the row
            Debug.Log($"No available spawn points left for row {randomRowIndex + 1}.");
        }
    }

    private IEnumerator GrowPad(PlatformController padObject)
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