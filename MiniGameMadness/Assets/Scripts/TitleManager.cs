using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    public string profileId; 
    public string brainCloudVersion;
    [SerializeField] private TitelInterface titelInterface;

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
        }
    }

    public void OnAuthenticationRequestCompleted(){
        brainCloudVersion =  Network.sharedInstance.brainCloudVersion;
        profileId = Network.sharedInstance.m_BrainCloud.GetStoredProfileId();
        Debug.Log("Signed in with Id: " + Network.sharedInstance.m_BrainCloud.GetStoredProfileId());
        titelInterface.UpdateInterface();
    }

    public void HandleLogOutButtonClick(){
        Network.sharedInstance.Logout(OnBrainCloudLogOutCompleted);
    }

    public void OnBrainCloudLogOutCompleted(){
        profileId = "null";
        titelInterface.UpdateInterface();
    }
}
