using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnLowLevels : MonoBehaviour{
    public int minimumLevel;
    void Start(){
        if(Metadata.LevelNumber < minimumLevel) Destroy(gameObject);
    }
}