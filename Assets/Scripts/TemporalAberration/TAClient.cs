using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TAClient : MonoBehaviour{
    public TAServer server;
    public float localTimeScale;

    public Rigidbody2D rb;

    void FixedUpdate(){
        // calculate local time scale
        localTimeScale = 1 + Mathf.Pow((server.transform.position - transform.position).magnitude * server.distanceScale, .5f) * server.timeScale;

        // apply local time scale to rigidbody
        rb.velocity *= localTimeScale;
    }
}
