using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PopulateEntityUI : MonoBehaviour
{
    public GameObject entityPrefab;
    private EntitiesManager entitiesManager;
    
    void Start()
    {
        entitiesManager = GameObject.FindObjectOfType<EntitiesManager>();
    }

    public void PopulateEntityContainer(){
        foreach (Modifier entity in entitiesManager.Modifiers)
        {
            GameObject spwaned = Instantiate(entityPrefab, gameObject.transform.position, Quaternion.identity, gameObject.transform);
            spwaned.GetComponent<TextMeshProUGUI>().text = entity.Title + " = " + entity.Value;
        }
    }
}
