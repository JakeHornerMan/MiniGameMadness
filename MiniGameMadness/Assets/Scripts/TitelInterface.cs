using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TitelInterface : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI profileId;
    [SerializeField] private TextMeshProUGUI versionText;
    [SerializeField] private GameObject authWindow;
    [SerializeField] private GameObject usernameWindow;
    [SerializeField] public TMP_InputField userIDField;
    [SerializeField] public TMP_InputField passwordField;
    [SerializeField] public TMP_InputField usernameField;
    [SerializeField] private GameObject logOutBtn;
    [SerializeField] private GameObject logInBtn;
    [SerializeField] private GameObject changeUsernameBtn;
    [SerializeField] private GameObject leaderBoardUI;
    

    [Header("Script References")]
    [SerializeField] private TitleManager titleManager;

    void Start()
    {
        titleManager = FindObjectOfType<TitleManager>();
        UpdateInterface();
    }

    public void UpdateInterface()
    {
        profileId.text = "Logged in as: " + titleManager.profileId;
        versionText.text = "BrainCloud: " + titleManager.brainCloudVersion;
    }

    public void OpenAuthWindow(){
        authWindow.SetActive(true);
        if(!authWindow.activeSelf){
            handleAuth();
        }
    }

    public void handleAuth(Network.AuthenticationRequestCompleted authenticationRequestCompleted = null, Network.AuthenticationRequestFailed authenticationRequestFailed = null){
        if(!authWindow.activeSelf){
            authWindow.SetActive(true);
            titleManager.Set(authenticationRequestCompleted, authenticationRequestFailed);
        }
    }

    public void closeAuthWindow(){
        authWindow.SetActive(false);
    }

    public void openUsernameWindow(bool isOpen){
        usernameWindow.SetActive(isOpen);
    }

    public void openLeaderBoardUI(bool isOpen){
        leaderBoardUI.SetActive(isOpen);
    }
    

    public void HandleButtons(bool loggedIn){
        if(loggedIn){
            logInBtn.SetActive(false);
            logOutBtn.SetActive(true);
            changeUsernameBtn.SetActive(true);
        }
        else{
            logInBtn.SetActive(true);
            logOutBtn.SetActive(false);
            changeUsernameBtn.SetActive(false);
        }
    }
}
