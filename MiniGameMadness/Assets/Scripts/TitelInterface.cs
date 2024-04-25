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
}
