using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitiesManager : MonoBehaviour
{
    public static EntitiesManager sharedInstance;
    private List<Modifier> m_Modifiers;

    private void Awake(){
        sharedInstance = this;
        m_Modifiers = new List<Modifier>();
    }
    
    public List<Modifier> Modifiers
    {
        get { return m_Modifiers; }
        set { m_Modifiers = value; }
    }

    public void AddToModifier(Modifier modifier){
        m_Modifiers.Add(modifier);
        Debug.Log("Entity added: " + modifier.Title);
    }
}

public class Modifier
{
    private string m_Title;
    private int m_Value;

    public Modifier(string title, int value){
        m_Title = title;
        m_Value = value;
    }

    public string Title
    {
        get { return m_Title; }
        set { m_Title = value; }
    }

    public int Value
    {
        get { return m_Value; }
        set { m_Value = value; }
    }
}
