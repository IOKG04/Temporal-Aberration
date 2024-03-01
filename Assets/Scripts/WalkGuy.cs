using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkGuy : MonoBehaviour{
    public Rigidbody2D rb;

    public Rigidbody2D target;
    public float distanceToTarget, distanceMaxDifference;
    public float speed;
    private Vector2 lastVelocity;
    private bool accelerating; // used as switch between accelerating and decelerating, just dont take this name too literally, look at the code before u change it
    public float accelerationTimer, accelerationTime;

    public TAServer taServer;
    private float localTimeScale;

    void Start(){
        rb = gameObject.GetComponent<Rigidbody2D>();
    }
    void FixedUpdate(){
        // calculate local time scale
        localTimeScale = 1 + Mathf.Pow((taServer.transform.position - transform.position).magnitude * taServer.distanceScale, 0.5f) * taServer.timeScale;

        // calculate movement
        Vector2 targetPosition = (rb.position - target.position).normalized * distanceToTarget + target.position;
        Vector2 targetVelocity;
        if((rb.position - targetPosition).magnitude > distanceMaxDifference){
            targetVelocity = (targetPosition - (Vector2)transform.position).normalized * speed;
            if(!accelerating){
                accelerating = true;
                accelerationTimer = 0f;
                lastVelocity = rb.velocity;
            }
        }
        else{
            targetVelocity = Vector2.zero;
            if(accelerating){
                accelerating = false;
                accelerationTimer = 0f;
                lastVelocity = rb.velocity;
            }
        }
        accelerationTimer += Time.fixedDeltaTime * localTimeScale;
        rb.velocity = Vector2.Lerp(lastVelocity, targetVelocity, Mathf.Sqrt(accelerationTimer / accelerationTime)) * localTimeScale;
    }
}
