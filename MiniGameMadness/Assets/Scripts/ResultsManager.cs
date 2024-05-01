using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultsManager : MonoBehaviour
{
    private LeaderboardsManager leaderboardsManager;
    private PopulateLeaderBoardUI populateLeaderBoardUI;

    void Awake(){
        leaderboardsManager = GameObject.FindObjectOfType<LeaderboardsManager>();
        populateLeaderBoardUI = GameObject.FindObjectOfType<PopulateLeaderBoardUI>();
    }

    void Start()
    {
        Network.sharedInstance.RequestLeaderboard(Constants.kBrainCloudDeadliftLeaderboardID, OnLeaderboardRequestCompleted);
    }

    private void OnLeaderboardRequestCompleted(Leaderboard leaderboard){
        leaderboardsManager.AddLeaderboard(leaderboard);
        HandleDeadliftLeaderBoard();
    }

    public Leaderboard GetLeaderboard(string LeaderboardID){
        Leaderboard leaderboard = leaderboardsManager.GetLeaderboardByName(LeaderboardID);
        Debug.Log(leaderboard.Name);
        return leaderboard;
    }

    public void HandleDeadliftLeaderBoard(){
        Leaderboard leaderboard = GetLeaderboard(Constants.kBrainCloudDeadliftLeaderboardID);
        populateLeaderBoardUI.PopulateContentContainer(leaderboard);
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
