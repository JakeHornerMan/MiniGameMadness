using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultsManager : MonoBehaviour
{
    private LeaderboardsManager leaderboardsManager;
    private PopulateLeaderBoardUI populateLeaderBoardUI;
    public GameObject leaderboardPanel;

    void Awake(){
        leaderboardsManager = GameObject.FindObjectOfType<LeaderboardsManager>();
        populateLeaderBoardUI = GameObject.FindObjectOfType<PopulateLeaderBoardUI>();
        leaderboardsManager.RefreshLeaderboards();
    }

    public void HandleDeadliftLeaderBoard(){
        leaderboardPanel.SetActive(true);
        populateLeaderBoardUI = GameObject.FindObjectOfType<PopulateLeaderBoardUI>();
        populateLeaderBoardUI.OnAllTimeButtonClick();
    }

    public void GoToScene(string scene){
        switch(scene) 
        {
        case "Deadlift":
            SceneManager.LoadScene("Deadlift");
            break;
        case "TitleScreen":
            SceneManager.LoadScene("TitleScreen");
            break;
        default:
            Debug.Log("invalid scene name");
            break;
        }
    }
}
