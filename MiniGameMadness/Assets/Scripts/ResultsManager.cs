using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultsManager : MonoBehaviour
{
    [SerializeField] private LeaderboardsManager leaderboardsManager;
    void Awake(){
        leaderboardsManager = GameObject.FindObjectOfType<LeaderboardsManager>();
    }

    void Start()
    {
        // Network.sharedInstance.RequestLeaderboard(Constants.kBrainCloudDeadliftLeaderboardID, OnLeaderboardRequestCompleted);
        GetLeaderboard(Constants.kBrainCloudDeadliftLeaderboardID);
    }

    // private void OnLeaderboardRequestCompleted(Leaderboard leaderboard){
    //     leaderboardsManager.AddLeaderboard(leaderboard);
    // }

    public Leaderboard GetLeaderboard(string leaderboardId){
        Leaderboard leaderboard = leaderboardsManager.GetLeaderboardByName(leaderboardId);
        Debug.Log(leaderboard.Name);
        return leaderboard;
    }
}
