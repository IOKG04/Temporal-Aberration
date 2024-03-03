using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HPMonitorController : MonoBehaviour{
    public PlayerController pc;
    public TextMeshProUGUI tmp;

    void FixedUpdate(){
        tmp.text = "HP: " + pc.hitPoints.ToString();
    }
}
