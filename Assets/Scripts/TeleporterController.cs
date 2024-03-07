using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Linq.Expressions;

public class TeleporterController : MonoBehaviour{
    public string targetScene, targetText;
    public GameObject textBox, interactableIndicator;
    public bool check;

    void Update(){
        if(check && Input.GetButtonDown("interact")){
            SceneManager.LoadSceneAsync(targetScene, LoadSceneMode.Single);
            textBox.SetActive(true);
            TextMeshProUGUI tmp = textBox.GetComponentInChildren<TextMeshProUGUI>();
            tmp.text = "Teleporting to:\n" + targetText;
        }
    }
    void OnTriggerEnter2D(Collider2D col){
        if(col.CompareTag("Player")){
            check = true;
            interactableIndicator.SetActive(true);
        }
    }
    void OnTriggerExit2D(Collider2D col){
        if(col.CompareTag("Player")){
            check = false;
            interactableIndicator.SetActive(false);
        }
    }
}
