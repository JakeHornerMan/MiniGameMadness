using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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
        Debug.Log("Signed in with Id: " + Network.sharedInstance.m_BrainCloud.GetStoredProfileId());
    }
}
