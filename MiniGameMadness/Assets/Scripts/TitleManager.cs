using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    public string profileId; 
    public string brainCloudVersion;
    [SerializeField] private TitelInterface titelInterface;

    private Network.AuthenticationRequestCompleted m_AuthenticationRequestCompleted;
    private Network.AuthenticationRequestFailed m_AuthenticationRequestFailed;

    void Start(){
        HandleAuthentication();
    }

    public void HandleAuthentication(){
        Debug.Log("Handle Authentication");
        if(Network.sharedInstance.HasAuthenticatedPreviously()){
            Debug.Log("Reconnect Authentication");
            Network.sharedInstance.Reconnect(OnAuthenticationRequestCompleted);
        }
        else{
            Debug.Log("Anonymous Authentication");
            Network.sharedInstance.RequestAnonymousAuthentication(OnAuthenticationRequestCompleted);
            // titelInterface.OpenAuthWindow();
        }
    }

    public void OnAuthenticationRequestCompleted(){
        brainCloudVersion =  Network.sharedInstance.brainCloudVersion;
        profileId = Network.sharedInstance.data.emailAddress;
        // profileId = Network.sharedInstance.m_BrainCloud.GetStoredProfileId()
        Debug.Log("Signed in with Id: " + profileId);
        titelInterface.UpdateInterface();
    }

    public void HandleLogOutButtonClick(){
        Network.sharedInstance.Logout(OnBrainCloudLogOutCompleted);
    }

    public void OnBrainCloudLogOutCompleted(){
        profileId = Network.sharedInstance.m_BrainCloud.GetStoredProfileId();
        Debug.Log("Logged out");
        titelInterface.UpdateInterface();
    }

    public void Set(Network.AuthenticationRequestCompleted authenticationRequestCompleted, Network.AuthenticationRequestFailed authenticationRequestFailed){
        m_AuthenticationRequestCompleted = authenticationRequestCompleted;
        m_AuthenticationRequestFailed = authenticationRequestFailed;
    }

    public void HandleLogInUniversalButtonClick(){
        Network.sharedInstance.RequestAuthenticationUniversal(titelInterface.userIDField.text, titelInterface.passwordField.text);
        OnAuthenticationRequestCompleted();
        titelInterface.closeAuthWindow();
        // string storedProfileId = Network.sharedInstance.m_BrainCloud.GetStoredAnonymousId();
        // if(storedProfileId != null){
        //     Debug.Log("We have a StoredProfileId: " + storedProfileId);
        // }
    }

    public void HandleLogInEmailButtonClick(){
        Network.sharedInstance.RequestAuthenticationEmail(titelInterface.userIDField.text, titelInterface.passwordField.text);
        OnAuthenticationRequestCompleted();
        titelInterface.closeAuthWindow();
    }
}
