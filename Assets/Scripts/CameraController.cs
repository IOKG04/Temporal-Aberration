using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class CameraController : MonoBehaviour{
    public Transform target;
    public float zOffset;

    void Start(){
    }
    void Update(){
        Vector3 finalTarget = target.position + new Vector3(0, 0, zOffset);
        transform.position = finalTarget;
    }
}
