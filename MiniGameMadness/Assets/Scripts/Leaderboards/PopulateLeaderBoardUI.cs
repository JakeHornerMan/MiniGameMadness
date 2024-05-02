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

    
    public void PopulateContentContainer(Leaderboard leaderboard){
        titleText.text = leaderboard.Name;
        foreach (LeaderboardEntry entry in leaderboard.EntryList)
        {
            Debug.Log("nickname:" + entry.Nickname + ", score:" + entry.Score + ", Rank:" + entry.Rank);
            GameObject spwaned = Instantiate(entryPrefab, contentContainer.transform.position, Quaternion.identity, contentContainer.transform);
            spwaned.GetComponent<EntryUi>().obj = entry;
            spwaned.GetComponent<EntryUi>().Populate();
        }

    }

    //TODO: rest score board for daily data
}
