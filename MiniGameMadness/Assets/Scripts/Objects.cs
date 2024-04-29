using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Currency
{
}

[System.Serializable]
public class Data
{
    public int abTestingId { get; set; }
    public long lastLogin { get; set; }
    public long server_time { get; set; }
    public int refundCount { get; set; }
    public int timeZoneOffset { get; set; }
    public int experiencePoints { get; set; }
    public int maxBundleMsgs { get; set; }
    public long createdAt { get; set; }
    public object parentProfileId { get; set; }
    public string emailAddress { get; set; }
    public int experienceLevel { get; set; }
    public string countryCode { get; set; }
    public int vcClaimed { get; set; }
    public Currency currency { get; set; }
    public string id { get; set; }
    public int compressIfLarger { get; set; }
    public int amountSpent { get; set; }
    public long previousLogin { get; set; }
    public string playerName { get; set; }
    public object pictureUrl { get; set; }
    public List<object> incoming_events { get; set; }
    public string sessionId { get; set; }
    public string languageCode { get; set; }
    public int vcPurchased { get; set; }
    public bool isTester { get; set; }
    public object summaryFriendData { get; set; }
    public int loginCount { get; set; }
    public bool emailVerified { get; set; }
    public bool xpCapped { get; set; }
    public string profileId { get; set; }
    public string newUser { get; set; }
    public int playerSessionExpiry { get; set; }
    public List<object> sent_events { get; set; }
    public int maxKillCount { get; set; }
    public Rewards rewards { get; set; }
    public Statistics statistics { get; set; }
}

[System.Serializable]
public class RewardDetails
{
}

[System.Serializable]
public class Rewards
{
    public RewardDetails rewardDetails { get; set; }
    public Currency currency { get; set; }
    public Rewards rewards { get; set; }
}

[System.Serializable]
public class Root
{
    public Data data { get; set; }
    public int status { get; set; }
}

[System.Serializable]
public class Statistics
{
}
