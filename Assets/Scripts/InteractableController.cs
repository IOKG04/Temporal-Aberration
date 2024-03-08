using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Timeline;

public class InteractableController : MonoBehaviour{
    public GameObject textBox, interactableIndicator;
    public bool check, deactivateNext;
    public int taMacro, taMini;
    public LineCollection[] lines;

    void Update(){
        if(check && Input.GetButtonDown("interact")){
            if(deactivateNext) Deactivate();
            else Activate();
        }
    }
    void OnTriggerStay2D(Collider2D col){
        if(col.CompareTag("Player") && interactableIndicator.GetComponent<InteractableIndicator>().activeIC == null){
            check = true;
            interactableIndicator.SetActive(true);
            interactableIndicator.GetComponent<InteractableIndicator>().activeIC = this;
        }
    }
    void OnTriggerExit2D(Collider2D col){
        if(col.CompareTag("Player")){
            check = false;
            Deactivate();
            interactableIndicator.SetActive(false);
            taMini = 0;
            interactableIndicator.GetComponent<InteractableIndicator>().activeIC = null;
        }
    }

    public void Activate(){
        TextMeshProUGUI tmp = textBox.GetComponentInChildren<TextMeshProUGUI>();
        tmp.text = lines[taMacro].l[taMini];
        textBox.SetActive(true);

        taMini++;
        if(taMini >= lines[taMacro].l.Length){
            taMini = 0;
            taMacro = (taMacro + 1) % lines.Length;
            deactivateNext = true;
        }
    }
    public void Deactivate(){
        textBox.SetActive(false);
        deactivateNext = false;
    }
}

[System.Serializable]
public struct LineCollection{
    public string[] l;
}