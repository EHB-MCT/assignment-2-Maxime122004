using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPositionManager : MonoBehaviour
{
    [SerializeField] private GameObject deathPinPrefab; // Prefab for the death position pin
    [SerializeField] private Transform pinParent; // Parent object for organizational purposes

    void Start()
    {
        DatabaseManager.Instance.FetchAndShowDeathPositions();
    }

    /**
     * Spawns death position pins based on an array of position strings.
     * Input: List<string> of death positions in the format "(x, y, z)".
     * Action: Parses positions and spawns pins at each location.
     * Output: None
     */
    public void SpawnDeathPins(List<string> deathPositions)
    {
        foreach (string positionString in deathPositions)
        {
            Debug.Log("pin spawned");
            Vector3 position = ParsePositionString(positionString);
            if (position != Vector3.zero) // Ensure parsing was successful
            {
                Instantiate(deathPinPrefab, position, Quaternion.identity);
            }
        }
    }

    /**
     * Parses a position string in the format "(x, y, z)" into a Vector3.
     * Input: A string in the format "(x, y, z)".
     * Action: Cleans the string and extracts the coordinates.
     * Output: A Vector3 with the parsed coordinates.
     */
    private Vector3 ParsePositionString(string positionString)
    {
        try
        {
            // Remove parentheses and spaces, then split by commas
            string cleanedString = positionString.Trim('(', ')');
            Debug.Log(cleanedString);
            string[] coordinates = cleanedString.Split(',');

            if (coordinates.Length == 3)
            {
                float x = float.Parse(coordinates[0].Replace(".", ","));
                float y = float.Parse(coordinates[1].Replace(".", ","));
                float z = float.Parse(coordinates[2].Replace(".", ","));
                return new Vector3(x, y, z);
            }
            else
            {
                Debug.LogWarning($"Invalid position string: {positionString}");
                return Vector3.zero;
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error parsing position string '{positionString}': {ex.Message}");
            return Vector3.zero;
        }
    }
}
