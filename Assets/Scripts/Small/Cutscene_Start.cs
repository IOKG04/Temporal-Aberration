using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Cutscene : MonoBehaviour{
    public InteractableController ic;
    public PlayerController pc;
    public GameObject pressEToContinue;
    public Image FadeImage;
    public float startingTimer, startingTime, fadeTime;

    void Start(){
        pc.enabled = false;
        ic.enabled = false;
    }
    void FixedUpdate(){
        startingTimer += Time.fixedDeltaTime;
        FadeImage.color = new Color(FadeImage.color.r, FadeImage.color.g, FadeImage.color.b, 1 - startingTimer / fadeTime);
        if(startingTimer > startingTime){
            if(ic.enabled == false){
                FadeImage.gameObject.SetActive(false);
                pressEToContinue.SetActive(true);
                ic.enabled = true;
                ic.check = true;
                ic.Activate();
            }
            else{
                if(ic.taMini >= 2) pressEToContinue.SetActive(false);
                if(ic.taMini == 0){
                    ic.Deactivate();
                    ic.enabled = false;
                    pc.enabled = true;
                    enabled = false;
                }
            }
        }
    }
}