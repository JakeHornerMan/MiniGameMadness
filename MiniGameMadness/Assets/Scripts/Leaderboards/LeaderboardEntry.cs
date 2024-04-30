using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardEntry
{
    private string m_Nickname;
    private int m_Score;
    private int m_Rank;
    private bool m_IsUserScore;


    public LeaderboardEntry(string nickname, int rank, int score)
    {
        m_Nickname = nickname;
        m_Rank = rank;
        m_Score = score;
    }

    public string Nickname
    {
        get { return m_Nickname; }
        set { m_Nickname = value; }
    }

    public int Score
    {
        get { return m_Score; }
        set { m_Score = value; }
    }

    public int Rank
    {
        get { return m_Rank; }
        set { m_Rank = value; }
    }

    public bool IsUserScore
    {
        get { return m_IsUserScore; }
        set { m_IsUserScore = value; }
    }
}
