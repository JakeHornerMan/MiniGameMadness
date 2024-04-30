using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DeadliftInterfaceManager : MonoBehaviour
{
    public GameObject successText;
    public GameObject failText;
    public GameObject goText;
    public TextMeshProUGUI infoText;
    public TextMeshProUGUI countDownText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;
    public GameObject nextRoundBtn;
    public GameObject proceedBtn;

    private void Start(){
       
    }

    public void SetTimer(float time){
        timerText.text = time.ToString();
    }

    public void UpdateWeight(int weight){
        infoText.text = weight + " KG";
    }

    public void SetSuccessActive(){
        successText.SetActive(true);
        successText.GetComponent<Animator>().Play("TextPopIn");
        StartCoroutine(AwayAndHide(2f, successText));
    }

    public void SetFailActive(){
        failText.SetActive(true);
        failText.GetComponent<Animator>().Play("TextPopIn");
        StartCoroutine(AwayAndHide(2f, failText));
    }

    public void SetGoActive(){
        goText.SetActive(true);
        goText.GetComponent<Animator>().Play("TextPopIn");
        StartCoroutine(AwayAndHide(0.6f, goText));
    }

    public IEnumerator AwayAndHide(float time, GameObject obj)
    {
        yield return new WaitForSeconds(time);
        obj.GetComponent<Animator>().Play("TextAway");
        yield return new WaitForSeconds(0.4f);
        obj.SetActive(false);
    }

    public void SetCountDown(int number, string option = null){
        countDownText.text = number.ToString();
        if(option != null)
            countDownText.text = option;
    }

    public void SetNextRoundButton(bool show){
        nextRoundBtn.SetActive(show);
    }

    public void SetProceedButton(bool show){
        proceedBtn.SetActive(show);
    }

    public void SetScore(int score){
        scoreText.text = score.ToString();
    }
}
