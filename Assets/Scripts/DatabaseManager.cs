using System.Collections.Generic;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Instance;
    private DatabaseReference _databaseReference;

    private string userID;
    public TMP_InputField Name;
    public TMP_InputField ScoreboardName;


    public TextMeshProUGUI nameScoreboard;
    public TextMeshProUGUI timeScore;
    public TextMeshProUGUI respawnScore;
    public TextMeshProUGUI jumpScore;

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

        // ScoreboardName = GameObject.Find("ScoreboardInputField").GetComponent<TMP_InputField>();
        // if (ScoreboardName != null)
        // {
        //     Debug.Log("scoreboardname found");
        // }
        // nameScoreboard = GameObject.Find("ScoreboardUserName").GetComponent<TextMeshProUGUI>();
        // if (nameScoreboard != null)
        // {
        //     Debug.Log("nameScoreboard found");
        // }
        // timeScore = GameObject.Find("TimeData").GetComponent<TextMeshProUGUI>();
        // if (timeScore != null)
        // {
        //     Debug.Log("timeScore found");
        // }
        // respawnScore = GameObject.Find("RespawnData").GetComponent<TextMeshProUGUI>();
        // if (respawnScore != null)
        // {
        //     Debug.Log("respawnScore found");
        // }
        // jumpScore = GameObject.Find("JumpData").GetComponent<TextMeshProUGUI>();
        // if (jumpScore != null)
        // {
        //     Debug.Log("jumpScore found");
        // }
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
     * Updates the best time for the user if the new time is better.
     */
    public void SaveBestTime(float newTime)
    {
        string userName = Name.text;

        _databaseReference.Child("users").Child(userName).Child("data").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot.Exists && snapshot.Child("bestTime").Value != null)
                {
                    Debug.Log("bestTime not found");
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
                    Debug.Log("bestTime found");
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
        string userName = Name.text;

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

    /** 
     * Saves data to Firebase.
     * @param key: The key under which the data is saved.
     * @param value: The data to save.
     */
    public void SaveData(string key, object value)
    {
        _databaseReference.Child("users").Child(Name.text).Child("data").Child(key).SetValueAsync(value).ContinueWithOnMainThread(task =>
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
        _databaseReference.Child(Name.text).Child(key).GetValueAsync().ContinueWithOnMainThread(task =>
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

    public void SaveUserData(string bestTime, List<Vector3> deathPositions)
    {
        Dictionary<string, object> userData = new Dictionary<string, object>
        {
            { "bestTime", bestTime },
            { "deathPositions", deathPositions }
        };

        _databaseReference.Child("users").Child(Name.text).Child("data").SetValueAsync(userData).ContinueWithOnMainThread(setTask =>
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
        _databaseReference.Child("users").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                User newUser = new User(Name.text);
                string userJson = JsonUtility.ToJson(newUser);

                Dictionary<string, object> userData = new Dictionary<string, object>
                    {
                        { "bestTime", "" },
                        { "deathPositions", "" }
                    };



                _databaseReference.Child("users").Child(Name.text).Child("data").SetValueAsync(userData).ContinueWithOnMainThread(setTask =>
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

    public void OpenGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    /** 
     * Gets userdata from Firebase.
     */
    public void GetUserData()
    {
        _databaseReference.Child("users").Child(ScoreboardName.text).Child("data").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot dataSnapshot = task.Result;

                if (dataSnapshot.Exists)
                {
                    var username = ScoreboardName.text;
                    var jumpAmount = dataSnapshot.Child("jumpAmount").Value;
                    var respawnAmount = dataSnapshot.Child("respawnAmount").Value;
                    var time = dataSnapshot.Child("time").Value;

                    nameScoreboard.text = username.ToString();
                    timeScore.text = time.ToString();
                    respawnScore.text = respawnAmount.ToString();
                    jumpScore.text = jumpAmount.ToString();
                }
                else
                {
                    nameScoreboard.text = "No User Found";
                    timeScore.text = 0.ToString();
                    respawnScore.text = 0.ToString();
                    jumpScore.text = 0.ToString();
                }
            }
            else
            {
                Debug.LogError($"Error retrieving data for this user: {task.Exception}");
            }
        });
    }
}