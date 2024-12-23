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

    void Update()
    {
        if (databaseManager == null)
        {
            databaseManager = FindAnyObjectByType<DatabaseManager>();
        }
    }
    
    public void OpenGame()
    {
        string userName = Name.text;

        if (userName != "")
        {
            SceneManager.LoadScene("SampleScene");
        }
        else
        {
            Debug.Log("Enter a username first");
        }
    }

    public void SearchUser()
    {
        databaseManager.GetUserData();
    }
}
