using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    /**
    * Handles Back To Home button at the end of the parkour.
    * Inputs: None.
    * Actions: Loads HomeScreen scene.
    * Outputs: None
    */
    public void BackToHome()
    {
        SceneManager.LoadSceneAsync("HomeScreen");
    }
}
