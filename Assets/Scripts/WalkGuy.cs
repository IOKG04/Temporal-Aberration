using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkGuy : MonoBehaviour{
    public Rigidbody2D rb;

    public Transform target;
    public float speed;

    void Start(){
    }
    void FixedUpdate(){
        rb.velocity = (target.position - transform.position).normalized * speed;
    }
}
