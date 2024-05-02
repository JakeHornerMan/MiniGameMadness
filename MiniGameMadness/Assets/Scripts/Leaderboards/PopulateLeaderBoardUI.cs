using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PopulateLeaderBoardUI : MonoBehaviour
{
    public GameObject entryPrefab;
    public GameObject contentContainer;
    [SerializeField] private TextMeshProUGUI titleText;
    private LeaderboardsManager leaderboardsManager;
    public GameObject leaderboardPanel;

    void Awake(){
        leaderboardsManager = GameObject.FindObjectOfType<LeaderboardsManager>();
    }
    
    public void PopulateContentContainer(Leaderboard leaderboard){
        titleText.text = leaderboard.Name;
        foreach (LeaderboardEntry entry in leaderboard.EntryList)
        {
            // Debug.Log("nickname:" + entry.Nickname + ", score:" + entry.Score + ", Rank:" + entry.Rank);
            GameObject spwaned = Instantiate(entryPrefab, contentContainer.transform.position, Quaternion.identity, contentContainer.transform);
            spwaned.GetComponent<EntryUi>().obj = entry;
            spwaned.GetComponent<EntryUi>().Populate();
        }
    }

    public void RepopulateContentContainer(Leaderboard leaderboard){
        foreach (Transform child in contentContainer.transform) {
            Destroy(child.gameObject);
        }
        PopulateContentContainer(leaderboard);
    }

    public void OnAllTimeButtonClick(){
        Leaderboard leaderboard = leaderboardsManager.GetLeaderboardByName(Constants.kBrainCloudDeadliftLeaderboardID);
        RepopulateContentContainer(leaderboard);
    }

    public void OnDailyButtonClick(){
        Leaderboard leaderboard = leaderboardsManager.GetLeaderboardByName(Constants.kBrainCloudDailyLeaderboardID);
        RepopulateContentContainer(leaderboard);
    }

    public void ClosePanel(){
        leaderboardPanel.SetActive(false);
    }
}
