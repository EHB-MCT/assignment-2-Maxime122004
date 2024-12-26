using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class Stopwatch : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    private float currentTime;
    private bool isPaused;
    public TimeSpan time;

    /**
     * Initializes the stopwatch in a paused state with a time of 0 seconds.
     * Inputs: None
     * Actions: Sets `isPaused` to true and `currentTime` to 0.
     * Outputs: None
     */
    private void Awake()
    {
        isPaused = true;
        currentTime = 0f;
    }

    /**
     * Updates the stopwatch time every frame when not paused and displays the elapsed time.
     * Inputs: Delta time (automatically provided by Unity).
     * Actions: Increments `currentTime` if the stopwatch is not paused, converts time to `TimeSpan`, and updates the timer UI.
     * Outputs: None
     */
    private void Update()
    {
        if (!isPaused)
        {
            currentTime += Time.deltaTime;
        }
        time = TimeSpan.FromSeconds(currentTime);
        timerText.text = "Time: " + time.TotalSeconds.ToString();
    }

    /**
     * Stops the stopwatch by pausing the time increment.
     * Inputs: None
     * Actions: Sets `isPaused` to true.
     * Outputs: None
     */
    public void StopTimer()
    {
        isPaused = true;
    }

    /**
     * Starts the stopwatch when the player enters the trigger zone.
     * Inputs: Collider of the object triggering the event.
     * Actions: Checks if the object is the player and resumes the stopwatch.
     * Outputs: None
     */
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "Player")
        {
            isPaused = false;
        }
    }
}