using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using LitJson;

public class Network : MonoBehaviour
{
    public string brainCloudVersion;
    public string email;
    public string username;
    public Data data;
    public static Network sharedInstance;
    public BrainCloudWrapper m_BrainCloud;
    public delegate void AuthenticationRequestCompleted();
    public delegate void AuthenticationRequestFailed();
    public delegate void BrainCloudLogOutCompleted();
    public delegate void BrainCloudLogOutFailed();
    public delegate void UpdateUsernameRequestCompleted();
    public delegate void UpdateUsernameRequestFailed();


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

    public bool HasAuthenticatedPreviously(){
        return m_BrainCloud.GetStoredProfileId() != "" && m_BrainCloud.GetStoredAnonymousId() != "";
    }

    public void RequestAnonymousAuthentication(AuthenticationRequestCompleted authenticationRequestCompleted = null,
    AuthenticationRequestFailed authenticationRequestFailed = null){
        BrainCloud.SuccessCallback successCallback = (responseData, cbObject) =>
        {
            Debug.Log("RequestAnonymousAuthentication success: " + responseData);
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
                username = "";
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

    private void HandleAuthenticationSuccess(string responseData, object cbObject, AuthenticationRequestCompleted authenticationRequestCompleted){
        JsonData json = JsonMapper.ToObject(responseData);
        username = json["data"]["playerName"].ToString();
        username = json["data"]["emailAddress"].ToString();
        Debug.Log("email: " + email + ", username: " + username);
        if(authenticationRequestCompleted != null)
            authenticationRequestCompleted();
    }

}
