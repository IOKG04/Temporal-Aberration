using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Timeline;

public class InteractableController : MonoBehaviour{
    public GameObject textBox;
    public bool check, active;
    public int timesActivated;
    public string[] lines;

    void Update(){
        if(check && Input.GetButtonDown("interact")){
            active = !active;
            if(active) Activate();
            else Deactivate();
        }
    }
    void OnTriggerEnter2D(Collider2D col){
        if(col.CompareTag("Player")){
            check = true;
        }
    }
    void OnTriggerExit2D(Collider2D col){
        if(col.CompareTag("Player")){
            check = false;
            active = false;
            Deactivate();
        }
    }

    void Activate(){
        TextMeshProUGUI tmp = textBox.GetComponentInChildren<TextMeshProUGUI>();
        tmp.text = lines[timesActivated];
        textBox.SetActive(true);

        timesActivated++;
        timesActivated %= lines.Length;
    }
    void Deactivate(){
        textBox.SetActive(false);
    }
}