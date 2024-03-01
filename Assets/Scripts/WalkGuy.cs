using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class WalkGuy : MonoBehaviour{
    public Rigidbody2D rb;

    public Rigidbody2D target;
    public float distanceToTarget, distanceMaxDifference;
    public float speed;
    private Vector2 lastVelocity;
    private bool accelerating; // used as switch between accelerating and decelerating, just dont take this name too literally, look at the code before u change it
    public float accelerationTimer, accelerationTime;

    public CircleCollider2D trigger;
    public float viewCheckTimer, viewCheckTime;

    public bool active;

    public TAServer taServer;
    private float localTimeScale;

    void Start(){
        rb = gameObject.GetComponent<Rigidbody2D>();
        distanceMaxDifference *= Random.Range(0.9f, 1.1f);
        accelerationTime *= Random.Range(0.95f, 1.05f);
        speed *= Random.Range(0.9f, 1.1f);
        viewCheckTimer = Random.Range(-0.2f, 0f);
        viewCheckTime *= Random.Range(0.95f, 1.05f);
        trigger = gameObject.GetComponent<CircleCollider2D>();
        trigger.radius *= Random.Range(0.9f, 1.1f);
    }
    void FixedUpdate(){
        // calculate local time scale
        localTimeScale = taServer.LocalTimeScale(transform.position);

        if(active){
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

            // check for player visibility
            viewCheckTimer += Time.fixedDeltaTime * localTimeScale;
            if(viewCheckTimer > viewCheckTime){
                viewCheckTimer = 0f;
                bool hitPlayer = false;
                RaycastHit2D[] hits = Physics2D.RaycastAll(rb.position, target.position - rb.position, trigger.radius);
                for(int i = 0; i < hits.Length; i++){
                    if(hits[i].collider.gameObject.CompareTag("Wall")) break;
                    if(hits[i].collider.gameObject.CompareTag("Player")){
                        hitPlayer = true;
                        break;
                    }
                }
                if(!hitPlayer){
                    active = false;
                    accelerationTimer = 0f;
                    lastVelocity = rb.velocity;
                }
            }
        }
        else{
            accelerationTimer += Time.fixedDeltaTime * localTimeScale;
            rb.velocity = Vector2.Lerp(lastVelocity, Vector2.zero, Mathf.Sqrt(accelerationTimer / accelerationTime)) * localTimeScale;
        }
    }
    void OnTriggerStay2D(Collider2D col){
        if(!active && col.gameObject.CompareTag("Player")){
            viewCheckTimer += Time.fixedDeltaTime * localTimeScale;
            if(viewCheckTimer > viewCheckTime){
                viewCheckTimer = 0f;
                RaycastHit2D[] hits = Physics2D.RaycastAll(rb.position, (Vector2)col.gameObject.transform.position - rb.position, trigger.radius);
                for(int i = 0; i < hits.Length; i++){
                    if(hits[i].collider.gameObject.CompareTag("Wall")) break;
                    if(hits[i].collider.gameObject.CompareTag("Player")){
                        active = true;
                        target = hits[i].collider.gameObject.GetComponent<Rigidbody2D>();
                        break;
                    }
                }
            }
        }
    }
}
