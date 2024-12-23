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

    private void Start()
    {
        userID = SystemInfo.deviceUniqueIdentifier;
        InitializeFirebase();
    }

    private void Update()
    {

    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "HomeScreen")
        {
            homescreenManager = FindAnyObjectByType<HomescreenManager>();
        }
    }

    /** 
     * Initializes Firebase and sets up the database reference.
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
     * Saves data to Firebase.
     * @param key: The key under which the data is saved.
     * @param value: The data to save.
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
     * Reads data from Firebase.
     * @param key: The key of the data to read.
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
     * Updates the best time for the user if the new time is better.
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
                        // Debug.Log("New best time saved: " + newTime);
                    }
                }
                else
                {
                    SaveData("bestTime", newTime);
                    // Debug.Log("Best time saved: " + newTime);
                }
            }
            else
            {
                Debug.LogError($"Error retrieving data: {task.Exception}");
            }

        });
    }

    /** 
    * Tracks the death position of the player and saves it to the database.
    */
    public void SaveDeathPosition(Vector3 deathPosition)
    {
        userName = homescreenManager.Name.text;

        // string x = deathPosition.x.ToString();
        // string y = deathPosition.y.ToString();
        // string z = deathPosition.z.ToString();
        // string position = x + y + z;

        string position = deathPosition.ToString();

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

    public void SaveAllData(float newTime, List<Vector3> deathPositions)
    {
        SaveBestTime(newTime);

        foreach (Vector3 position in deathPositions)
        {
            Debug.Log(position);
            SaveDeathPosition(position);
        }
    }

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
     * Adds userdata to Firebase.
     */
    public void CreateUser()
    {
        userName = homescreenManager.Name.text;

        _databaseReference.Child("users").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                User newUser = new User(userName);
                string userJson = JsonUtility.ToJson(newUser);

                Dictionary<string, object> userData = new Dictionary<string, object>
                    {
                        { "bestTime", "" },
                        { "deathPositions", "" }
                    };

                _databaseReference.Child("users").Child(userName).Child("data").SetValueAsync(userData).ContinueWithOnMainThread(setTask =>
                {
                    if (setTask.IsCompleted)
                    {
                        Debug.Log("User created successfully.");
                        SceneManager.LoadScene("SampleScene");
                    }
                    else
                    {
                        Debug.LogError("Error creating user: " + setTask.Exception);
                    }
                });
            }
            else
            {
                Debug.LogError("Error checking user existence: " + task.Exception);
            }
        });
    }

    /** 
     * Gets userdata from Firebase.
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
}