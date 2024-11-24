using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;

public class AnalyticsScript : MonoBehaviour
{
    public static AnalyticsScript Instance;
    private bool _isInitialized = false;

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

    private async void Start()
    {
        await UnityServices.InitializeAsync();
        AnalyticsService.Instance.StartDataCollection();
        _isInitialized = true;
    }

    public void JumpAmount(int amount)
    {
        if(!_isInitialized)
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
