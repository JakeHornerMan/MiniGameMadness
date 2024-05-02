using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardsManager : MonoBehaviour
{
    public static LeaderboardsManager sharedInstance;

    public List<Leaderboard> m_Leaderboards;
    private int m_UserScore;
    // private PopulateLeaderBoardUI populateLeaderBoardUI;

    private void Awake(){
        sharedInstance = this;
        m_Leaderboards = new List<Leaderboard>();
    }

    public void AddLeaderboard(Leaderboard leaderboard){
        if(m_UserScore > 0){
            for(int i = 0; i < m_Leaderboards.Count; i++){
                if(leaderboard.GetLeaderboardEntryAtIndex(i).Nickname == Network.sharedInstance.GetUserName()){
                    leaderboard.GetLeaderboardEntryAtIndex(i).IsUserScore = true;
                    break;
                }
            }
        }

        m_Leaderboards.RemoveAll(p => p.Name == leaderboard.Name);
        m_Leaderboards.Add(leaderboard);
        Debug.Log("Leaderboard added: " + leaderboard.Name);
    }

    public Leaderboard GetLeaderboardByName(string name){
        for(int i = 0; i < m_Leaderboards.Count; i++){
            if(m_Leaderboards[i].Name == name){
                // Debug.Log(m_Leaderboards[i].Name);
                return m_Leaderboards[i];
            }
        }
        return null;
    }

    public int GetCount(){
        return m_Leaderboards.Count;
    }

    public void RefreshLeaderboards(){
        string[] leaderboardIds = {Constants.kBrainCloudDeadliftLeaderboardID, Constants.kBrainCloudDailyLeaderboardID};
        Network.sharedInstance.GetLeaderBoardsByLeaderboardId(leaderboardIds, OnLeaderboardRequestCompleted);
    }

    private void OnLeaderboardRequestCompleted(List<Leaderboard> leaderboards){
        if(leaderboards.Count > 0){
            foreach(Leaderboard leaderboard in leaderboards){
                AddLeaderboard(leaderboard);
            }
        }
    }
}
