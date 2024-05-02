using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PopulateEntityUI : MonoBehaviour
{
    private EntitiesManager entitiesManager;
    List<GameObject> entities;
    public GameObject entityPrefab;
    
    void Start()
    {
        entitiesManager = GameObject.FindObjectOfType<EntitiesManager>();
        entities = new List<GameObject>();
    }

    public void PopulateEntityContainer(){
        foreach (Modifier entity in entitiesManager.Modifiers)
        {
            GameObject spwaned = Instantiate(entityPrefab, gameObject.transform.position, Quaternion.identity, gameObject.transform);
            spwaned.GetComponent<TextMeshProUGUI>().text = entity.Title + " = " + entity.Value;
        }
    }

}
