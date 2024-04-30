using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadliftManager : MonoBehaviour
{
    public DeadliftInterfaceManager dlInterface;
    public GameObject deadlifter;
    public List<int> deadLifts;
    public int roundNumber = -1;
    public int currentDeadlift;
    public int counterProgress = 0;
    public float timeAmount = 5.0f;
    public float timeleft;
    enum State { Rest, Progress, Success, Fail}
    State gameState = State.Success;
    enum Key { Left, Up, Right }
    Key currentKey = Key.Left;
    void Start()
    {
        
    }

    void Update()
    {
        if(gameState == State.Progress){
            DeadLifting();
            Timer();
        }
        if(gameState == State.Success){
            if (Input.GetKeyDown(KeyCode.Space))
                SetUpLift();
        }
        if(gameState == State.Fail){
            //end Game
        }
    }

    public void DeadLifting(){
        
        if (Input.GetKeyDown(KeyCode.LeftArrow) && currentKey == Key.Left)
        {
            counterProgress++;
            currentKey = Key.Up;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && currentKey == Key.Up)
        {
            counterProgress++;
            currentKey = Key.Right;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) && currentKey == Key.Right)
        {
            counterProgress++;
            currentKey = Key.Left;
        }

        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     counterProgress++;
        // }

        if(counterProgress >= currentDeadlift){
            gameState = State.Success;
            Debug.Log("YOU LIFTED!");
            deadlifter.GetComponent<Animator>().Play("success");
            dlInterface.SetSuccessActive();
            StartCoroutine(CountAndNextRound(2f));
        }
    }

    public void Timer(){
        if(timeleft > 0){
            timeleft -= Time.deltaTime;
            dlInterface.SetTimer(timeleft);
        }
        else{
            gameState = State.Fail;
            Debug.Log("YOU FAILED");
            dlInterface.SetFailActive();
        }
    }

    public void SetUpLift(){
        deadlifter.GetComponent<Animator>().Play("progress");
        dlInterface.SetNextRoundButton(false);
        roundNumber++;
        dlInterface.UpdateWeight(deadLifts[roundNumber]);
        currentDeadlift = deadLifts[roundNumber]/6;
        Debug.Log("targetNum: " + currentDeadlift);
        timeleft = timeAmount;
        counterProgress = 0;
        CountDown(3);
        Debug.Log("LETS LIFT");
    }

    public void CountDown(int number){
        StartCoroutine(CountAndWait(number));
    }

    public IEnumerator CountAndWait(int number)
    {
        dlInterface.SetCountDown(number);
        yield return new WaitForSeconds(1f);
        number--;
        if(number <= 0){
            dlInterface.SetGoActive();
            dlInterface.SetCountDown(number, ""); 
            gameState = State.Progress;
        }
        else{
            CountDown(number);
        } 
    }

    public IEnumerator CountAndNextRound(float number)
    {
        yield return new WaitForSeconds(number);
        dlInterface.SetNextRoundButton(true);
    }
}
