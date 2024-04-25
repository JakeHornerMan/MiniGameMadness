using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Network : MonoBehaviour
{
    public string brainCloudVersion;

    public static Network sharedInstance;
    public BrainCloudWrapper m_BrainCloud;
    public delegate void AuthenticationRequestCompleted();
    public delegate void AuthenticationRequestFailed();
    public delegate void BrainCloudLogOutCompleted();
    public delegate void BrainCloudLogOutFailed();
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

    private void HandleAuthenticationSuccess(string responseData, object cbObject, AuthenticationRequestCompleted authenticationRequestCompleted){
        if(authenticationRequestCompleted != null)
            authenticationRequestCompleted();
    }

    public void Reconnect(AuthenticationRequestCompleted authenticationRequestCompleted = null, 
        AuthenticationRequestFailed authenticationRequestFailed = null)
    {
        BrainCloud.SuccessCallback successCallback = (responseData, cbObject) =>
        {
            Debug.Log("ReconnectAnonymousAuthentication success: " + responseData);
            HandleAuthenticationSuccess(responseData, cbObject, authenticationRequestCompleted);
        };

        BrainCloud.FailureCallback failureCallback = (status, code, error, cbObject) =>
        {
            Debug.Log(string.Format("[Reconnect Failed] {0}  {1}  {2}", status, code, error));
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
                if(brainCloudLogOutCompleted != null)
                    brainCloudLogOutCompleted();
            };

            BrainCloud.FailureCallback failureCallback = (status, code, error, cbObject) =>
            {
                Debug.Log(string.Format("[Logout Failed] {0}  {1}  {2}", status, code, error));
                if(brainCloudLogOutFailed != null)
                    brainCloudLogOutFailed();
            };

            m_BrainCloud.Logout(true, successCallback, failureCallback);
        }
    }

}
