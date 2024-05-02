using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using LitJson;

public class Network : MonoBehaviour
{
    public delegate void AuthenticationRequestCompleted();
    public delegate void AuthenticationRequestFailed();
    public delegate void BrainCloudLogOutCompleted();
    public delegate void BrainCloudLogOutFailed();
    public delegate void UpdateUsernameRequestCompleted();
    public delegate void UpdateUsernameRequestFailed();
    public delegate void UpdateUserPictureUrlRequestCompleted();
    public delegate void UpdateUserPictureUrlRequestFailed();
    public delegate void LeaderboardRequestCompleted(Leaderboard leaderboard);
    public delegate void LeaderboardsRequestCompleted(List<Leaderboard> leaderboard);
    public delegate void LeaderboardRequestFailed();
    public delegate void PostScoreRequestCompleted();
    public delegate void PostScoreRequestFailed();
    public delegate void RequestGlobalEntityDataCompleted(List<Modifier> modifiers);
    public delegate void RequestGlobalEntityDataFailed();


    public string brainCloudVersion;
    public string email;
    public string username;
    public Data data;
    public static Network sharedInstance;
    public BrainCloudWrapper m_BrainCloud;


    void Awake(){
        Debug.Log("BrainCloud client starting...");
        sharedInstance = this;
        DontDestroyOnLoad(gameObject);

        //Create and initializethe BrainCloud wraooer
        m_BrainCloud = gameObject.AddComponent<BrainCloudWrapper>();
        m_BrainCloud.Init();

        brainCloudVersion = m_BrainCloud.Client.BrainCloudClientVersion;
        Debug.Log("BrainCloud client version: " + m_BrainCloud.Client.BrainCloudClientVersion);
    }

    void Update(){
        // Make sure you invoke this method in Update, or else you won't get any callbacks
        m_BrainCloud.RunCallbacks();
    }

    void OnApplicationQuit(){
        m_BrainCloud.Logout(false);
    }

    public string BrainCloudClientVersion{
        get { return m_BrainCloud.Client.BrainCloudClientVersion; }
    }

    public bool IsAuthenticated(){
        return m_BrainCloud.Client.Authenticated;
    }

     public string GetUserName(){
        return username;
    }

    public bool HasAuthenticatedPreviously(){
        return m_BrainCloud.GetStoredProfileId() != "" && m_BrainCloud.GetStoredAnonymousId() != "";
    }

    public void RequestAnonymousAuthentication(AuthenticationRequestCompleted authenticationRequestCompleted = null,
    AuthenticationRequestFailed authenticationRequestFailed = null){
        BrainCloud.SuccessCallback successCallback = (responseData, cbObject) =>
        {
            Debug.Log("RequestAnonymousAuthentication success: " + responseData);
            username = null;
            email = null;
            HandleAuthenticationSuccess(responseData, cbObject, authenticationRequestCompleted);
        };

        BrainCloud.FailureCallback failureCallback = (status, code, error, cbObject) =>
        {
            Debug.Log(string.Format("[Authenticate Failed] {0}  {1}  {2}", status, code, error));
            if(authenticationRequestFailed != null)
                authenticationRequestFailed();
        };

        m_BrainCloud.AuthenticateAnonymous(successCallback, failureCallback);
    }

    public void Reconnect(AuthenticationRequestCompleted authenticationRequestCompleted = null, 
        AuthenticationRequestFailed authenticationRequestFailed = null)
    {
        BrainCloud.SuccessCallback successCallback = (responseData, cbObject) =>
        {
            Debug.Log("ReconnectAuthentication success: " + responseData);
            username = null;
            email = null;
            HandleAuthenticationSuccess(responseData, cbObject, authenticationRequestCompleted);
        };

        BrainCloud.FailureCallback failureCallback = (status, code, error, cbObject) =>
        {
            Debug.LogError(string.Format("[Reconnect Failed] {0}  {1}  {2}", status, code, error));
            if(authenticationRequestFailed != null)
                authenticationRequestFailed();
        };

        m_BrainCloud.Reconnect(successCallback, failureCallback);
    }

    public void Logout(BrainCloudLogOutCompleted brainCloudLogOutCompleted = null, 
        BrainCloudLogOutFailed brainCloudLogOutFailed = null)
    {
        if(IsAuthenticated()){
            BrainCloud.SuccessCallback successCallback = (responseData, cbObject) =>
            {
                Debug.Log("Logout success: " + responseData);
                username = null;
                email = null;
                if(brainCloudLogOutCompleted != null)
                    brainCloudLogOutCompleted();
            };

            BrainCloud.FailureCallback failureCallback = (status, code, error, cbObject) =>
            {
                Debug.LogError(string.Format("[Logout Failed] {0}  {1}  {2}", status, code, error));
                if(brainCloudLogOutFailed != null)
                    brainCloudLogOutFailed();
            };

            m_BrainCloud.Logout(true, successCallback, failureCallback);
        }
        else{
            Debug.LogError("Logout Failed, there is no user autheticated");
            if(brainCloudLogOutFailed != null)
                    brainCloudLogOutFailed();
        }
    }

    public void RequestAuthenticationUniversal(string userID, string password, AuthenticationRequestCompleted authenticationRequestCompleted = null,
        AuthenticationRequestFailed authenticationRequestFailed = null)
    {
        BrainCloud.SuccessCallback successCallback = (responseData, cbObject) =>
        {
            Debug.Log("UniversalAuthentication success: " + responseData);
            username = null;
            email = null;
            HandleAuthenticationSuccess(responseData, cbObject, authenticationRequestCompleted);
        };

        BrainCloud.FailureCallback failureCallback = (status, code, error, cbObject) =>
        {
            Debug.LogError(string.Format("[UniversalAuthentication Failed] {0}  {1}  {2}", status, code, error));
            if(authenticationRequestFailed != null)
                authenticationRequestFailed();
        };

        m_BrainCloud.AuthenticateUniversal(userID, password, true, successCallback, failureCallback);
    }

    public void RequestAuthenticationEmail(string email, string password, AuthenticationRequestCompleted authenticationRequestCompleted = null,
        AuthenticationRequestFailed authenticationRequestFailed = null)
    {
        BrainCloud.SuccessCallback successCallback = (responseData, cbObject) =>
        {
            Debug.Log("EmailAuthentication success: " + responseData);
            username = null;
            email = null;
            HandleAuthenticationSuccess(responseData, cbObject, authenticationRequestCompleted);
        };

        BrainCloud.FailureCallback failureCallback = (status, code, error, cbObject) =>
        {
            Debug.LogError(string.Format("[EmailAuthentication Failed] {0}  {1}  {2}", status, code, error));
            if(authenticationRequestFailed != null)
                authenticationRequestFailed();
        };

        m_BrainCloud.AuthenticateEmailPassword(email, password, true, successCallback, failureCallback);
    }

    public void UpdateUserName(string userName, UpdateUsernameRequestCompleted updateUsernameRequestCompleted = null,
        UpdateUsernameRequestFailed updateUsernameRequestFailed = null)
    {
        if(IsAuthenticated()){
            BrainCloud.SuccessCallback successCallback = (responseData, cbObject) =>
            {
                Debug.Log("Username Update success: " + responseData);
                HandleAuthenticationSuccess(responseData, cbObject);

                if(updateUsernameRequestCompleted != null)
                    updateUsernameRequestCompleted();
            };

            BrainCloud.FailureCallback failureCallback = (status, code, error, cbObject) =>
            {
                Debug.LogError(string.Format("[Update Username Failed] {0}  {1}  {2}", status, code, error));
                if(updateUsernameRequestFailed != null)
                    updateUsernameRequestFailed();
            };

            m_BrainCloud.PlayerStateService.UpdateName(userName, successCallback, failureCallback);
        }
        else{
            Debug.LogError("Update Username Failed, there is no user autheticated");
            if(updateUsernameRequestFailed != null)
                    updateUsernameRequestFailed();
        }
    }

    public void UpdateUserPictureUrl(string profileUrl, UpdateUserPictureUrlRequestCompleted updateUserPictureUrlRequestCompleted = null,
        UpdateUserPictureUrlRequestFailed updateUserPictureUrlRequestFailed = null)
    {
        if(IsAuthenticated()){
            BrainCloud.SuccessCallback successCallback = (responseData, cbObject) =>
            {
                Debug.Log("Profile Image Update success: " + responseData);
                HandleAuthenticationSuccess(responseData, cbObject);

                if(updateUserPictureUrlRequestCompleted != null)
                    updateUserPictureUrlRequestCompleted();
            };

            BrainCloud.FailureCallback failureCallback = (status, code, error, cbObject) =>
            {
                Debug.LogError(string.Format("[Profile Image Update Failed] {0}  {1}  {2}", status, code, error));
                if(updateUserPictureUrlRequestFailed != null)
                    updateUserPictureUrlRequestFailed();
            };

            m_BrainCloud.PlayerStateService.UpdateUserPictureUrl(profileUrl, successCallback, failureCallback);
        }
        else{
            Debug.LogError("Profile Image Update Failed, there is no user autheticated");
            if(updateUserPictureUrlRequestFailed != null)
                    updateUserPictureUrlRequestFailed();
        }
    }

    private void HandleAuthenticationSuccess(string responseData, object cbObject, AuthenticationRequestCompleted authenticationRequestCompleted = null){
        JsonData json = JsonMapper.ToObject(responseData);
        if(responseData.Contains("playerName")){
            // Debug.Log("playerName found");
            username = json["data"]["playerName"].ToString();
        }
        if(responseData.Contains("emailAddress")){
            // Debug.Log("emailAddress found");
            email = json["data"]["emailAddress"].ToString();
        }
        // Debug.Log("email: " + email + ", username: " + username);
        if(authenticationRequestCompleted != null)
            authenticationRequestCompleted();
    }

    public void RequestLeaderboard(string leaderboardId, LeaderboardRequestCompleted leaderboardRequestCompleted = null,
        LeaderboardRequestFailed leaderboardRequestFailed = null)
    {
        RequestLeaderboard(leaderboardId, Constants.kBrainCloudDefaultMinHighScoreIndex,
            Constants.kBrainCloudDefaultMaxHighScoreIndex, leaderboardRequestCompleted, leaderboardRequestFailed);
    }

    public void RequestLeaderboard(string leaderboardId, int startIndex, int endIndex, 
        LeaderboardRequestCompleted leaderboardRequestCompleted = null, LeaderboardRequestFailed leaderboardRequestFailed = null)
    {
        if(IsAuthenticated()){
            BrainCloud.SuccessCallback successCallback = (responseData, cbObject) =>
            {
                Debug.Log(leaderboardId + " leaderboard request success: " + responseData);
                JsonData jsonData = JsonMapper.ToObject(responseData);
                JsonData leaderboard = jsonData["data"]["leaderboard"];

                List<LeaderboardEntry> leaderboardEntryList = new List<LeaderboardEntry>();
                int rank = 0;
                string nickname;
                int score = 0;

                if(leaderboard.IsArray){
                    for(int i = 0; i < leaderboard.Count; i++){
                        rank = int.Parse(leaderboard[i]["rank"].ToString());
                        nickname = leaderboard[i]["data"]["nickname"].ToString();
                        score = int.Parse(leaderboard[i]["score"].ToString());

                        leaderboardEntryList.Add(new LeaderboardEntry(nickname, rank, score));
                    }
                }
                Leaderboard lb = new Leaderboard(leaderboardId, leaderboardEntryList);

                if (leaderboardRequestCompleted != null)
                    leaderboardRequestCompleted(lb);
            };

            BrainCloud.FailureCallback failureCallback = (statusMessage, code, error, cbObject) =>
            {
                Debug.Log(string.Format("[" + leaderboardId + " leaderboard Request Failed] {0}  {1}  {2}", statusMessage, code, error));

                if (leaderboardRequestFailed != null)
                    leaderboardRequestFailed();
            };
            m_BrainCloud.LeaderboardService.GetGlobalLeaderboardPage(leaderboardId, BrainCloud.BrainCloudSocialLeaderboard.SortOrder.HIGH_TO_LOW, startIndex, endIndex, successCallback, failureCallback);
        }
        else{
            Debug.Log("Failed Request for leadreboard: " + leaderboardId + ". User is not Authenticated.");
            if(leaderboardRequestFailed != null)
                leaderboardRequestFailed();
        }
    }

    public void PostScoreToLeaderboard(string leaderboardID, int score, 
        PostScoreRequestCompleted postScoreRequestCompleted = null, PostScoreRequestFailed postScoreRequestFailed = null)
    {
        PostScoreToLeaderboard(leaderboardID, score, username, postScoreRequestCompleted, postScoreRequestFailed);
    }

    public void PostScoreToLeaderboard(string leaderboardID, int score, string nickname, 
        PostScoreRequestCompleted postScoreRequestCompleted = null, PostScoreRequestFailed postScoreRequestFailed = null)
    {
        if (IsAuthenticated()){
            BrainCloud.SuccessCallback successCallback = (responseData, cbObject) =>
            {
                Debug.Log(leaderboardID + " PostScoreToLeaderboard success: " + responseData);
                if (postScoreRequestCompleted != null)
                    postScoreRequestCompleted();
            };

            BrainCloud.FailureCallback failureCallback = (statusMessage, code, error, cbObject) =>
            {
                Debug.Log(string.Format("[" + leaderboardID + " PostScoreToLeaderboard Failed] {0}  {1}  {2}", statusMessage, code, error));
                if (postScoreRequestFailed != null)
                    postScoreRequestFailed();
            };

            string jsonOtherData = "{\"nickname\":\"" + nickname + "\"}";
            m_BrainCloud.LeaderboardService.PostScoreToLeaderboard(leaderboardID, score, jsonOtherData, successCallback, failureCallback);
        }
        else
        {
            Debug.Log("PostScoreToLeaderboard failed: user is not authenticated");

            if (postScoreRequestFailed != null)
                postScoreRequestFailed();
        }
    }

    public void RequestGlobalEntityData(string globalEntityIndexedID, RequestGlobalEntityDataCompleted requestGlobalEntityDataCompleted = null, 
        RequestGlobalEntityDataFailed requestGlobalEntityDataFailed = null){
        if (IsAuthenticated()){
            BrainCloud.SuccessCallback successCallback = (responseData, cbObject) =>
            {
                Debug.Log("RequestGlobalEntityData success: " + responseData);

                List<Modifier> modifiers = new List<Modifier>();
                JsonData jsonData = JsonMapper.ToObject(responseData);
                JsonData entityList = jsonData["data"]["entityList"];

                if (entityList.IsArray)
                {
                    for (int i = 0; i < entityList.Count; i++){
                        if(entityList[i]["entityIndexedId"].ToString() == "Modifier"){
                            // Debug.Log("Modifier!");
                            string title = entityList[i]["entityType"].ToString();
                            int value = int.Parse(entityList[i]["data"]["value"].ToString());
                            Modifier mod = new Modifier(title, value);
                            modifiers.Add(mod);
                        }
                    }
                }

                if (requestGlobalEntityDataCompleted != null)
                    requestGlobalEntityDataCompleted(modifiers);
            };

            BrainCloud.FailureCallback failureCallback = (statusMessage, code, error, cbObject) =>
            {
                Debug.Log("RequestGlobalEntityData failed: " + statusMessage);

                if (requestGlobalEntityDataFailed != null)
                    requestGlobalEntityDataFailed();
            };

            m_BrainCloud.GlobalEntityService.GetListByIndexedId(globalEntityIndexedID, 10, successCallback, failureCallback);
        }
        else
        {
            Debug.Log("RequestGlobalEntityData failed: user is not authenticated");

            if (requestGlobalEntityDataFailed != null)
                requestGlobalEntityDataFailed();
        }
    }

    public void PostScoreToLeaderboards(int score, PostScoreRequestCompleted postScoreRequestCompleted = null, PostScoreRequestFailed postScoreRequestFailed = null)
    {
        PostScoreToLeaderboards(score, username, postScoreRequestCompleted, postScoreRequestFailed);
    }

    public void PostScoreToLeaderboards(int score, string nickname, PostScoreRequestCompleted postScoreRequestCompleted = null, PostScoreRequestFailed postScoreRequestFailed = null)
    {
        if (IsAuthenticated())
        {
            BrainCloud.SuccessCallback successCallback = (responseData, cbObject) =>
            {
                Debug.Log("PostScoreToLeaderboards success: " + responseData);

                if (postScoreRequestCompleted != null)
                    postScoreRequestCompleted();
            };

            BrainCloud.FailureCallback failureCallback = (statusMessage, code, error, cbObject) =>
            {
                Debug.Log("PostScoreToLeaderboards failed: " + statusMessage);

                if (postScoreRequestFailed != null)
                    postScoreRequestFailed();
            };

            string jsonScriptData = "{\"leaderboards\":[\"Deadlift\", \"Daily\"],\"score\":" + score + ",\"extras\":{\"nickname\":\"" + nickname + "\"}}";


            m_BrainCloud.ScriptService.RunScript("PostToLeaderboards", jsonScriptData, successCallback, failureCallback);
        }
        else
        {
            Debug.Log("PostScoreToLeaderboards failed: user is not authenticated");

            if (postScoreRequestFailed != null)
                postScoreRequestFailed();
        }
    }

    public void GetLeaderBoardsByLeaderboardId(string[] leaderboardIds, LeaderboardsRequestCompleted leaderboardsRequestCompleted = null, 
        LeaderboardRequestFailed leaderboardRequestFailed = null)
    {
        if(IsAuthenticated()){
            BrainCloud.SuccessCallback successCallback = (responseData, cbObject) =>
            {
                Debug.Log("GetLeaderBoardsByLeaderboardId request success: " + responseData);
                JsonData jsonData = JsonMapper.ToObject(responseData);
                JsonData leaderboards = jsonData["data"]["response"]["leaderboards"];

                List<Leaderboard> leaderboardList = new List<Leaderboard>();

                if(leaderboards.IsArray){
                    for(int i = 0; i < leaderboards.Count; i++){

                        string leaderboardId = leaderboardIds[i];

                        JsonData leaderboard = leaderboards[i]["data"]["leaderboard"];

                        List<LeaderboardEntry> leaderboardEntryList = new List<LeaderboardEntry>();
                        int rank = 0;
                        string nickname;
                        int score = 0;

                        if(leaderboard.IsArray){
                            for(int j = 0; j < leaderboard.Count; j++){
                                rank = int.Parse(leaderboard[j]["rank"].ToString());
                                nickname = leaderboard[j]["data"]["nickname"].ToString();
                                score = int.Parse(leaderboard[j]["score"].ToString());

                                leaderboardEntryList.Add(new LeaderboardEntry(nickname, rank, score));
                            }
                        }
                        Leaderboard lb = new Leaderboard(leaderboardId, leaderboardEntryList);
                        leaderboardList.Add(lb);
                    }
                }

                if (leaderboardsRequestCompleted != null)
                    leaderboardsRequestCompleted(leaderboardList);
            };

            BrainCloud.FailureCallback failureCallback = (statusMessage, code, error, cbObject) =>
            {
                Debug.Log(string.Format("[GetLeaderBoardsByLeaderboardId Request Failed] {0}  {1}  {2}", statusMessage, code, error));

                if (leaderboardRequestFailed != null)
                    leaderboardRequestFailed();
            };

            string jsonScriptData = "{\"leaderboards\":[\"Deadlift\", \"Daily\"]}"; //TODO: stringbuilder
            m_BrainCloud.ScriptService.RunScript("GetLeaderBoardsByLeaderboardId", jsonScriptData, successCallback, failureCallback);

        }
        else{
            Debug.Log("Failed Request for GetLeaderBoardsByLeaderboardId. User is not Authenticated.");
            if(leaderboardRequestFailed != null)
                leaderboardRequestFailed();
        }
    }

}
