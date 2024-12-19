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
                }
            }
            else
            {
                Debug.LogError($"Error retrieving data for this user: {task.Exception}");
            }
        });
    }

}
