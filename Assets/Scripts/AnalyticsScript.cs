using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;

public class AnalyticsScript : MonoBehaviour
{
    public static AnalyticsScript Instance;
    private bool _isInitialized = false;

    /** 
    * Sets up the singleton instance of AnalyticsScript.
    * Inputs: None
    * Actions: Ensures only one instance of AnalyticsScript exists in the scene. 
    * Outputs: None
    */
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    /** 
     * Initializes Unity Services and starts data collection.
     * Inputs: None
     * Actions: Asynchronously initializes Unity Services and enables data collection for analytics. 
     * Outputs: None
     */
    private async void Start()
    {
        await UnityServices.InitializeAsync();
        AnalyticsService.Instance.StartDataCollection();
        _isInitialized = true;
    }

    /** 
     * Sends a custom analytics event for player jumps.
     * Inputs: The amount of jumps performed by the player (int amount).
     * Actions: Records a custom "jump" event with Unity Analytics and flushes the data. 
     * Outputs: None
     */
    public void JumpAmount(int amount)
    {
        if (!_isInitialized)
        {
            return;
        }
        CustomEvent myEvent = new CustomEvent("jump")
        {
            {"jump_index", amount}
        };
        AnalyticsService.Instance.RecordEvent(myEvent);
        AnalyticsService.Instance.Flush();
        Debug.Log("jump");
    }
}
