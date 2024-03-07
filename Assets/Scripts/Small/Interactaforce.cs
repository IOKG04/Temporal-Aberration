using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Timeline;

public class Interactaforce : MonoBehaviour{
    public GameObject textBox;
    public PlayerController pc;
    public bool check, deactivateNext;
    public int taMacro, taMini;
    public LineCollection[] lines;

    void Update(){
        if(check && Input.GetButtonDown("interact")){
            if(deactivateNext) Deactivate();
            else Activate();
        }
    }
    void OnTriggerEnter2D(Collider2D col){
        if(col.CompareTag("Player")){
            check = true;
            pc.enabled = false;
            pc.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            Activate();
        }
    }
    void OnTriggerExit2D(Collider2D col){
        if(col.CompareTag("Player")){
            check = false;
            Deactivate();
            taMini = 0;
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
        pc.enabled = true;
        gameObject.SetActive(false);
    }
}