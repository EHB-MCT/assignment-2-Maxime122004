using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float bestTime = float.MaxValue;
    public List<Vector3> deathPositions = new List<Vector3>();

    public void UpdateBestTime(float newTime)
    {
        if (newTime < bestTime)
        {
            bestTime = newTime;
            Debug.Log("New best time: " + bestTime);
        }
    }

    public void AddDeathPosition(Vector3 newDeathPosition)
    {
        deathPositions.Add(newDeathPosition);
        Debug.Log("Death position added: " + newDeathPosition);
    }

    public void DisplayDeathPositions()
    {
        foreach (var pos in deathPositions)
        {
            Debug.Log("Death position: " + pos);
        }
    }
}
