using System.Collections.Generic;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance;
    private DatabaseReference _databaseReference;

    private string userID;
    public TMP_InputField Name;

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
        _databaseReference.Child(key).GetValueAsync().ContinueWithOnMainThread(task =>
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

    public void CreateUser(string name)
    {
        _databaseReference.Child("users").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                // User does not exist, create a new user
                User newUser = new User(Name.text);
                string userJson = JsonUtility.ToJson(newUser);

                // Initial data for the user
                Dictionary<string, object> userData = new Dictionary<string, object>
                    {
                    { "jumpAmount", 0 },
                    { "respawnAmount", 0 },
                    { "time", "00:00:00.0000000" }
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
}
