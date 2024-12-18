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
    private TimeSpan time;

    private void Awake()
    {
        isPaused = true;
        currentTime = 0f;
    }

    private void Update()
    {
        if (!isPaused)
        {
            currentTime += Time.deltaTime;
        }
        time = TimeSpan.FromSeconds(currentTime);
        timerText.text = "Timer " + time.TotalSeconds.ToString();
    }

    public void StopTimer()
    {
        isPaused = true;
        FirebaseManager.Instance.SaveData("time", time.ToString());
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "Player")
        {
            isPaused = false;
        }
    }
}