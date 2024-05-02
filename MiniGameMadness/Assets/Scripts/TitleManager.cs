using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private LeaderboardsManager leaderboardsManager;
    [SerializeField] private EntitiesManager entitiesManager;
    public string profileId; 
    public string brainCloudVersion;
    [SerializeField] private TitelInterface titelInterface;

    private Network.AuthenticationRequestCompleted m_AuthenticationRequestCompleted;
    private Network.AuthenticationRequestFailed m_AuthenticationRequestFailed;

    private PopulateLeaderBoardUI populateLeaderBoardUI;

    void Start(){
        if(!Network.sharedInstance.IsAuthenticated())
            HandleAuthentication();
    }

    public void HandleAuthentication(){
        Debug.Log("Handle Authentication");
        if(Network.sharedInstance.HasAuthenticatedPreviously()){
            Debug.Log("Reconnect Authentication");
            Network.sharedInstance.Reconnect(OnAuthenticationRequestCompleted);
            titelInterface.HandleButtons(true);
        }
        else{
            Debug.Log("Anonymous Authentication");
            Network.sharedInstance.RequestAnonymousAuthentication(OnAnonAuthenticationRequestCompleted);
            titelInterface.HandleButtons(false);
        }
    }

    public void OnAnonAuthenticationRequestCompleted(){
        titelInterface.HandleButtons(false);
        brainCloudVersion =  Network.sharedInstance.brainCloudVersion;
        profileId = Network.sharedInstance.m_BrainCloud.GetStoredProfileId();
        Debug.Log("Signed in with Id: " + profileId);
        titelInterface.UpdateInterface();

        if(leaderboardsManager.GetCount() <= 0){
            leaderboardsManager.RefreshLeaderboards();
        }
        if(entitiesManager.GetCount() <= 0){
            Network.sharedInstance.RequestGlobalEntityData(Constants.kBrainCloudGlobalEntityIndexedID, OnGlobalEntityRequestCompleted);
        }        
    }

    public void OnAuthenticationRequestCompleted(){
        titelInterface.closeAuthWindow();
        titelInterface.HandleButtons(true);
        brainCloudVersion =  Network.sharedInstance.brainCloudVersion;
        if(Network.sharedInstance.username != ""){
            profileId = Network.sharedInstance.username;
        }
        else { 
            if(Network.sharedInstance.email != ""){
                profileId = Network.sharedInstance.email;
            }
            else{
                profileId = Network.sharedInstance.m_BrainCloud.GetStoredProfileId();
            }
        }
        
        Debug.Log("Signed in with Id: " + profileId);
        titelInterface.UpdateInterface();
        
        if(leaderboardsManager.GetCount() <= 0){
            leaderboardsManager.RefreshLeaderboards();
        }
        if(entitiesManager.GetCount() <= 0){
            Network.sharedInstance.RequestGlobalEntityData(Constants.kBrainCloudGlobalEntityIndexedID, OnGlobalEntityRequestCompleted);
        }  
    }

    public void HandleLogOutButtonClick(){
        Network.sharedInstance.Logout(OnBrainCloudLogOutCompleted);
    }

    public void OnBrainCloudLogOutCompleted(){
        profileId = Network.sharedInstance.m_BrainCloud.GetStoredProfileId();
        Debug.Log("Logged out");
        titelInterface.UpdateInterface();
        titelInterface.HandleButtons(false);
    }

    public void Set(Network.AuthenticationRequestCompleted authenticationRequestCompleted, Network.AuthenticationRequestFailed authenticationRequestFailed){
        m_AuthenticationRequestCompleted = authenticationRequestCompleted;
        m_AuthenticationRequestFailed = authenticationRequestFailed;
    }

    public void HandleLogInUniversalButtonClick(){
        Network.sharedInstance.RequestAuthenticationUniversal(titelInterface.userIDField.text, titelInterface.passwordField.text, OnAuthenticationRequestCompleted);
        OnAuthenticationRequestCompleted();
        titelInterface.closeAuthWindow();
    }

    public void HandleLogInEmailButtonClick(){
        Network.sharedInstance.RequestAuthenticationEmail(titelInterface.userIDField.text, titelInterface.passwordField.text, HandleLogInEmailSuccess, HandleLogInEmailFail);
    }

    public void HandleLogInEmailFail(){
        Debug.Log("All fields are required.");
        titelInterface.SetAuthErrorText("All fields are required.");
    }

    public void HandleLogInEmailSuccess(){
        if(titelInterface.changeUsernameField.text != null){
            Network.sharedInstance.UpdateUserName(titelInterface.usernameField.text, OnAuthenticationRequestCompleted, HandleLogInEmailFail);
        }
    }

    public void HandleSetUserNameButtonClick(){
        if(titelInterface.changeUsernameField.text != null){
            Network.sharedInstance.UpdateUserName(titelInterface.changeUsernameField.text, OnAuthenticationRequestCompleted);
            Network.sharedInstance.UpdateUserPictureUrl("https://source.unsplash.com/user/c_v_r");
            titelInterface.openUsernameWindow(false);
        }
        else{
            Debug.Log("Usrename field is required!");
        }
    }

    public void GoToScene(string scene){
        switch(scene) 
        {
        case "Deadlift":
            SceneManager.LoadScene("Deadlift");
            break;
        default:
            Debug.Log("invalid scene name");
            break;
        }
    }

    private void OnGlobalEntityRequestCompleted(List<Modifier> modifiers){
        if(modifiers.Count > 0){
            foreach(Modifier modifier in modifiers){
                entitiesManager.AddToModifier(modifier);
            }
        }
    }

    public Leaderboard GetLeaderboard(string LeaderboardID){
        Leaderboard leaderboard = leaderboardsManager.GetLeaderboardByName(LeaderboardID);
        Debug.Log(leaderboard.Name);
        return leaderboard;
    }

    public void HandleDeadliftLeaderBoard(){
        Leaderboard leaderboard = GetLeaderboard(Constants.kBrainCloudDeadliftLeaderboardID);
        titelInterface.openLeaderBoardUI(true);
        populateLeaderBoardUI = GameObject.FindObjectOfType<PopulateLeaderBoardUI>();
        populateLeaderBoardUI.RepopulateContentContainer(leaderboard);
    }
}
