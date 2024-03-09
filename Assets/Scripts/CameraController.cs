using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

/*
Screen shake code heavily inspired by the answer by Martin-Schulz in
https://forum.unity.com/threads/screen-shake-effect.22886/
*/

public class CameraController : MonoBehaviour{
    public Transform target;
    public float zOffset;
    public Camera thisCamera;
    public Transform parent;
    public float screenShakeStrength, screenShakeTimer;
    public int screenShakeFrameCounter, screenShakeFrameCount; // in physics frames

    void Start(){
        thisCamera = gameObject.GetComponent<Camera>();
    }
    void Update(){
        Vector3 finalTarget = target.position + new Vector3(0, 0, zOffset);
        transform.localPosition = finalTarget;
    }
    void FixedUpdate(){
        if(screenShakeFrameCounter <= 0){
            if(screenShakeTimer > 0){
                parent.localPosition = Random.insideUnitSphere * screenShakeStrength
         * screenShakeTimer;
                screenShakeTimer -= Time.fixedDeltaTime * screenShakeFrameCount;
                if(screenShakeTimer < 0){
                    screenShakeTimer = 0;
                    parent.localPosition = Vector2.zero;
                }
            }
            screenShakeFrameCounter = screenShakeFrameCount;
        }
        screenShakeFrameCounter--;
    }
    public void ShakeScreen(float strength, float time){
        screenShakeStrength = strength;
        screenShakeTimer = time;
    }
}
