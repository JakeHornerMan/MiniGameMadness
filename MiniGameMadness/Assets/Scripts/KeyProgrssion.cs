using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyProgrssion : MonoBehaviour
{
    public GameObject upKey;
    public GameObject downKey;
    public GameObject leftKey;
    public GameObject rightKey;

    public enum GameType { DeadLift, Other }
    public GameType gametype = GameType.DeadLift;
    public enum Key { Left, Up, Right, Down, Space }
    public Key currentKey = Key.Left;
    private void Start(){
        if(gametype == GameType.DeadLift){
            currentKey = Key.Left;
            DeadLiftKeyProgrssion();
        }
    } 

    public void DeadLiftKeyProgrssion(){
        GameObject key = null;
        if(currentKey == Key.Left){
            key = leftKey;
        }
        if(currentKey == Key.Up){
            key = upKey;
        }
        if(currentKey == Key.Right){
            key = rightKey;
        }
        StartCoroutine(PlayAndWait(key));
    }

    public IEnumerator PlayAndWait(GameObject key)
    {
        key.GetComponent<Animator>().Play("Press");
        yield return new WaitForSeconds(0.6f);
        if(gametype == GameType.DeadLift){
            if(currentKey == Key.Left){
                currentKey = Key.Up;
                DeadLiftKeyProgrssion();
                yield break;
            }
            if(currentKey == Key.Up){
                currentKey = Key.Right;
                DeadLiftKeyProgrssion();
                yield break;
            }
            if(currentKey == Key.Right){
                currentKey = Key.Left;
                DeadLiftKeyProgrssion();
                yield break;
            }
        }
    }
}
