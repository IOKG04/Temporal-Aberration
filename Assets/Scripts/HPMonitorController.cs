using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HPMonitorController : MonoBehaviour{
    public PlayerController pc;
    public TextMeshProUGUI tmp;
    private float EETimer;

    void FixedUpdate(){
        tmp.text = "HP: " + pc.hitPoints.ToString();

        // easter egg
        if(EETimer >= 0f) EETimer += Time.fixedDeltaTime;
        if(EETimer > 1f){
            EETimer = 0f;
            if(GameObject.FindGameObjectsWithTag("WalkGuy").Length <= 0) EETimer = -1f;
        }
        if(EETimer < 0f) tmp.text = "Easter egg :3";
    }
}
