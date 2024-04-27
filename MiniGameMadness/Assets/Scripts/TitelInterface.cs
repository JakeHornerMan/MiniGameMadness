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
    [SerializeField] public TMP_InputField userIDField;
    [SerializeField] public TMP_InputField passwordField;
    

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
}
