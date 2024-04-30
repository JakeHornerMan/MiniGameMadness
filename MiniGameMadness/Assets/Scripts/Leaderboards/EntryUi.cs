using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EntryUi : MonoBehaviour
{
    public LeaderboardEntry obj;
    [SerializeField] private TextMeshProUGUI rankText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI scoreText;

    public void Populate(){
        rankText.text = obj.Rank.ToString();
        nameText.text = obj.Nickname.ToString();
        scoreText.text = obj.Score.ToString();
    }
    
}
