using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadliftManager : MonoBehaviour
{
    public DeadliftInterfaceManager dlInterface;
    public GameObject deadlifter;
    public List<int> deadLifts;
    public int roundNumber = 0;
    public int currentDeadlift;
    public int counterProgress = 0;
    public float timeAmount = 5.0f;
    public float timeleft;
    enum State { Rest, Progress, Success, Fail}
    State gameState;
    enum Key { Left, Up, Right }
    Key currentKey = Key.Left;
    void Start()
    {
        SetUpLift();
    }

    void Update()
    {
        if(gameState == State.Progress){
            DeadLifting();
            Timer();
        }
        if(gameState == State.Success){
            //set new lift
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

        if(counterProgress >= currentDeadlift){
            gameState = State.Success;
            Debug.Log("YOU LIFTED!");
            deadlifter.GetComponent<Animator>().Play("success");
            dlInterface.SetSuccessActive();
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
        dlInterface.UpdateWeight(deadLifts[roundNumber]);
        currentDeadlift = deadLifts[roundNumber]/5;
        timeleft = timeAmount;
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
}
