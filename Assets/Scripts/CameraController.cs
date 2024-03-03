using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class CameraController : MonoBehaviour{
    public Transform target;
    public float zOffset;

    public Camera thisCamera;

    void Start(){
        thisCamera = gameObject.GetComponent<Camera>();
        //thisCamera.aspect = 16f / 9f;
    }
    void Update(){
        Vector3 finalTarget = target.position + new Vector3(0, 0, zOffset);
        transform.position = finalTarget;
    }
}
