using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadliftManager : MonoBehaviour
{
    public DeadliftInterfaceManager dlInterface;
    private EntitiesManager entitiesManager;
    public GameObject deadlifter;
    public List<int> deadLifts;
    public int roundNumber = -1;
    public int currentDeadlift;
    public int counterProgress = 0;
    public float timeAmount = 5.0f;
    public float timeleft;
    public int playerScore = 0;
    enum State { Rest, Progress, Success, Fail, Finished}
    State gameState = State.Success;
    enum Key { Left, Up, Right }
    Key currentKey = Key.Left;
    public Vector3 originalPosition;

    private int gravity = 8;
    private int bonusMultiplier = 0;

    void Start()
    {
        originalPosition = deadlifter.transform.position;
        entitiesManager = GameObject.FindObjectOfType<EntitiesManager>();
        SetEntitysForLevel();
    }

    public void SetEntitysForLevel(){
        if(entitiesManager.Modifiers.Count <= 0){
            gravity = 8;
            Debug.Log("Gravity: " + gravity);
            bonusMultiplier = 1;
            Debug.Log("BonusMultiplier: " + bonusMultiplier);
            return;
        }
        foreach(Modifier modifier in entitiesManager.Modifiers){
            if(modifier.Title == "Gravity"){
                gravity = modifier.Value;
                Debug.Log("Gravity: " + gravity);
            } 
            if(modifier.Title == "BonusMultiplier"){
                bonusMultiplier = modifier.Value;
                Debug.Log("BonusMultiplier: " + bonusMultiplier);
            }
        }
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
        if(gameState == State.Finished){
            //end Game
        }
    }

    public void DeadLifting(){
        Shake();
        if(counterProgress >= currentDeadlift){
            gameState = State.Success;
            deadlifter.transform.position = originalPosition;
            Debug.Log("YOU LIFTED!");
            CaculatePoints();
            deadlifter.GetComponent<Animator>().Play("success");
            dlInterface.SetSuccessActive();
            if(roundNumber + 1 >= deadLifts.Count){
                HandleScorePost();
                gameState = State.Finished;
                dlInterface.SetProceedButton(true);
            }
            else{
                StartCoroutine(CountAndNextRound(1f));
            }
        }
        
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
    }

    public void Timer(){
        if(timeleft > 0){
            timeleft -= Time.deltaTime;
            dlInterface.SetTimer(timeleft);
        }
        else{
            gameState = State.Fail;
            Debug.Log("YOU FAILED");
            HandleScorePost();
            dlInterface.SetFailActive();
            gameState = State.Finished;
            dlInterface.SetProceedButton(true);
        }
    }

    public void SetUpLift(){
        dlInterface.SetNextRoundButton(false);
        deadlifter.GetComponent<Animator>().Play("progress");
        roundNumber++;
        dlInterface.UpdateWeight(deadLifts[roundNumber]);
        currentDeadlift = deadLifts[roundNumber]/gravity;
        Debug.Log("targetNum: " + currentDeadlift);
        timeleft = timeAmount;
        counterProgress = 0;
        CountDown(3);
        Debug.Log("LETS LIFT");
    }

    public void CaculatePoints(){
        int add;
        add = deadLifts[roundNumber] * 100;
        add += (int)(timeleft * 100)*bonusMultiplier;
        Debug.Log("Pointes added: " + add);
        playerScore += add;
        dlInterface.SetScore(playerScore);
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

    public void Shake(){
        float offsetX = Random.Range(-1f, 1f) * counterProgress/100;
        float offsetY = Random.Range(-1f, 1f) * counterProgress/100;
        deadlifter.transform.position = originalPosition + new Vector3(offsetX, offsetY, 0);
    }

    public void LoadResults(){
        SceneManager.LoadScene("Results");
    }

    public void HandleScorePost(){
        Network.sharedInstance.PostScoreToLeaderboards(playerScore);
    }
}
