using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomescreenManager : MonoBehaviour
{
    public TMP_InputField Name;
    public TMP_InputField ScoreboardName;


    public TextMeshProUGUI nameScoreboard;
    public TextMeshProUGUI timeScore;
    public TextMeshProUGUI deathCountScore;

    public Button startButton;
    public Button searchButton;
    public DatabaseManager databaseManager;
    
    /**
     * Assigns GameObject with type DatabaseManager to databaseManager variable.
     * Inputs: None
     * Actions: Retrieves GameObject with type DatabaseManager.
     * Outputs: None
     */
    void Update()
    {
        if (databaseManager == null)
        {
            databaseManager = FindAnyObjectByType<DatabaseManager>();
        }
    }

    /**
     * Loads Level scene. 
     * Inputs: None
     * Actions: Loads Level scene. 
     * Outputs: None
     */
    public void OpenGame()
    {
        string userName = Name.text;

        if (userName != "")
        {
            SceneManager.LoadScene("Level");
        }
        else
        {
            Debug.Log("Enter a username first");
        }
    }

    /**
     * Calls the GetUserData() function from the DatabaseManager script.
     * Inputs: None
     * Actions:  Calls GetUserData() function. 
     * Outputs: None
     */
    public void SearchUser()
    {
        databaseManager.GetUserData();
    }
}
