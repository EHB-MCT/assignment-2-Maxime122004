using System.Collections.Generic;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Instance;
    private DatabaseReference _databaseReference;

    private string userID;
    private string userName;
    private string scoreboardName;

    public HomescreenManager homescreenManager;
    public DeathPositionManager deathPositionManager;

    /**
     * Ensures only one instance of the DatabaseManager exists and persists across scenes.
     * Inputs: None
     * Actions: Sets `Instance` to this instance if none exists; otherwise, destroys the duplicate object.
     * Outputs: None
     */
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /**
     * Initializes user ID and Firebase setup.
     * Inputs: None
     * Actions: Retrieves unique device ID and initializes Firebase.
     * Outputs: None
     */
    private void Start()
    {
        userID = SystemInfo.deviceUniqueIdentifier;
        InitializeFirebase();
    }

    /**
     * Subscribes to scene load events when the object is enabled.
     * Inputs: None
     * Actions: Adds the `OnSceneLoaded` method to the sceneLoaded event.
     * Outputs: None
     */
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    /**
     * Unsubscribes from scene load events when the object is disabled.
     * Inputs: None
     * Actions: Removes the `OnSceneLoaded` method from the sceneLoaded event.
     * Outputs: None
     */
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /**
     * Handles actions when a new scene is loaded.
     * Inputs: The loaded scene and its load mode.
     * Actions: Finds the HomescreenManager instance if the HomeScreen scene is loaded.
     * Outputs: None
     */
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "HomeScreen")
        {
            homescreenManager = FindAnyObjectByType<HomescreenManager>();
        }
        else if (scene.name == "Level")
        {
            deathPositionManager = FindAnyObjectByType<DeathPositionManager>();

        }
    }

    /**
     * Initializes Firebase and sets up the database reference.
     * Inputs: None
     * Actions: Checks Firebase dependencies and initializes the database reference.
     * Outputs: None
     */
    private void InitializeFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                _databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
                Debug.Log("Firebase initialized successfully.");
            }
            else
            {
                Debug.LogError($"Could not resolve Firebase dependencies: {task.Result}");
            }
        });
    }

    /**
     * Saves data to Firebase under a specified key.
     * Inputs: The key and value to store in Firebase.
     * Actions: Writes the data to Firebase under the user's name.
     * Outputs: None
     */
    public void SaveData(string key, object value)
    {
        userName = homescreenManager.Name.text;

        _databaseReference.Child("users").Child(userName).Child("data").Child(key).SetValueAsync(value).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log($"Data saved successfully: {key} = {value}");
            }
            else
            {
                Debug.LogError($"Error saving data: {task.Exception}");
            }
        });
    }

    /**
     * Retrieves data from Firebase for a specified key.
     * Inputs: The key to retrieve from Firebase.
     * Actions: Reads the data from Firebase and logs it.
     * Outputs: None
     */
    public void GetData(string key)
    {
        userName = homescreenManager.Name.text;

        _databaseReference.Child(userName).Child(key).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && task.Result.Value != null)
            {
                Debug.Log($"Data retrieved: {key} = {task.Result.Value}");
            }
            else
            {
                Debug.LogWarning($"No data found for key: {key}");
            }
        });
    }

    /**
     * Updates the user's best time if the new time is better.
     * Inputs: The new best time float value.
     * Actions: Saves the new best time if it is better than the current one.
     * Outputs: None
     */
    public void SaveBestTime(float newTime)
    {
        userName = homescreenManager.Name.text;

        _databaseReference.Child("users").Child(userName).Child("data").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot.Exists && snapshot.Child("bestTime").Value != null)
                {
                    float currentBestTime = float.Parse(snapshot.Child("bestTime").Value.ToString());
                    Debug.Log(newTime);
                    Debug.Log(currentBestTime);
                    if (newTime < currentBestTime)
                    {
                        SaveData("bestTime", newTime);
                    }
                }
                else
                {
                    SaveData("bestTime", newTime);
                }
            }
            else
            {
                Debug.LogError($"Error retrieving data: {task.Exception}");
            }

        });
    }

    /**
     * Saves the player's death position to Firebase.
     * Inputs: A Vector3 representing the death position.
     * Actions: Writes the position data to Firebase under the user's data.
     * Outputs: None
     */
    public void SaveDeathPosition(Vector3 deathPosition)
    {
        userName = homescreenManager.Name.text;
        string position = deathPosition.ToString();

        // string x = deathPosition.x.ToString();
        // string y = deathPosition.y.ToString();
        // string z = deathPosition.z.ToString();
        // string position = x + y + z;

        _databaseReference.Child("users").Child(userName).Child("data").Child("deathPositions").Push().SetValueAsync(position).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Firebase connection successful");
            }
            else
            {
                Debug.LogError("Firebase connection failed: " + task.Exception);
            }
        });

    }

    /** 
     * Saves all relevant user data to Firebase.
     * Inputs: `newTime` (float), the new best time; `deathPositions` (List<Vector3>), the list of death positions.
     * Actions: Saves the new best time and iterates through death positions to save them.
     * Outputs: None
     */
    public void SaveAllData(float newTime, List<Vector3> deathPositions)
    {
        SaveBestTime(newTime);

        foreach (Vector3 position in deathPositions)
        {
            Debug.Log(position);
            SaveDeathPosition(position);
        }
    }

    /** 
     * Saves the user data to Firebase as a dictionary.
     * Inputs: `bestTime` (string), the user's best time; `deathPositions` (List<Vector3>), the list of death positions.
     * Actions: Stores the user data in Firebase as a dictionary.
     * Outputs: None
     */
    public void SaveUserData(string bestTime, List<Vector3> deathPositions)
    {
        userName = homescreenManager.Name.text;

        Dictionary<string, object> userData = new Dictionary<string, object>
        {
            { "bestTime", bestTime },
            { "deathPositions", deathPositions }
        };

        _databaseReference.Child("users").Child(userName).Child("data").SetValueAsync(userData).ContinueWithOnMainThread(setTask =>
        {
            if (setTask.IsCompleted)
            {
                Debug.Log("User created successfully.");
            }
            else
            {
                Debug.LogError("Error creating user: " + setTask.Exception);
            }
        });
    }

    /** 
     * Retrieves user data from Firebase and displays it on the home screen.
     * Inputs: None
     * Actions: Fetches user data and updates the home screen's scoreboard fields.
     * Outputs: None
     */
    public void GetUserData()
    {
        scoreboardName = homescreenManager.ScoreboardName.text;
        Debug.Log(scoreboardName);
        _databaseReference.Child("users").Child(scoreboardName).Child("data").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot dataSnapshot = task.Result;

                if (dataSnapshot.Exists)
                {
                    var usernameScoreboard = scoreboardName;
                    var time = dataSnapshot.Child("bestTime").Value;
                    var deathCountData = dataSnapshot.Child("deathPositions");
                    int deathCount = 0;

                    if (deathCountData.Exists && deathCountData.Value is Dictionary<string, object> deathPositions)
                    {
                        deathCount = deathPositions.Count;
                    }

                    homescreenManager.nameScoreboard.text = usernameScoreboard.ToString();
                    homescreenManager.timeScore.text = time.ToString();
                    homescreenManager.deathCountScore.text = deathCount.ToString();
                }
                else
                {
                    homescreenManager.nameScoreboard.text = "No User Found";
                    homescreenManager.timeScore.text = 0.ToString();
                    homescreenManager.deathCountScore.text = 0.ToString();
                }
            }
            else
            {
                Debug.LogError($"Error retrieving data for this user: {task.Exception}");
            }
        });
    }

    // public void GetUserDeaths()
    // {
    //     userName = homescreenManager.Name.text;
    //     _databaseReference.Child("users").Child(userName).Child("data").Child("deathPositions").GetValueAsync().ContinueWithOnMainThread(task =>{
    //         if (task.IsCompleted)
    //         {
    //             DataSnapshot snapshot = task.Result;

    //             if (snapshot.Exists && snapshot.Value != null)
    //             {
    //                 Debug.Log(snapshot);
    //             }
    //         }
    //         else
    //         {
    //             Debug.LogError($"Error retrieving data: {task.Exception}");
    //         }
    //     });
    // }

    /**
     * Fetches death positions from the database for the given player and spawns pins.
     * Input: Player ID.
     * Action: Retrieves death positions and spawns pins using the DeathPositionManager.
     * Output: None.
     */
    public void FetchAndShowDeathPositions()
    {
        userName = homescreenManager.Name.text;
        string path = $"players/{userName}/deathPositions";

        _databaseReference.Child("users").Child(userName).Child("data").Child("deathPositions").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && task.Result.Exists)
            {
                List<string> deathPositions = new List<string>();
                Debug.Log(task.Result);
                foreach (var item in task.Result.Children)
                {
                    string positionString = item.Value.ToString();
                    deathPositions.Add(positionString);
                }

                // Call the DeathPositionManager to spawn pins
                deathPositionManager.SpawnDeathPins(deathPositions);
            }
            else
            {
                Debug.LogWarning($"No death positions found for player: {userName}");
            }
        });
    }
}