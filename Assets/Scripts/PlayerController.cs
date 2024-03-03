using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour{
    public Rigidbody2D rb;

    public Vector2 velocityTarget, velocityLastChange, velocityChangeTimer;
    public float velocityChangeTime;
    public float speed;
    private bool changedDirectionX, changedDirectionY; // was a direction button pressed this physics frame

    public int hitPoints, maxHitPoints;

    void Start(){
        rb = gameObject.GetComponent<Rigidbody2D>();
        hitPoints = maxHitPoints;
    }

    void Update(){
        // set downs
        changedDirectionX = //changedDirectionX ||
                           Input.GetButtonDown("left") || Input.GetButtonDown("right") ||
                           Input.GetButtonUp("left") || Input.GetButtonUp("right");
        changedDirectionY = //changedDirectionY ||
                           Input.GetButtonDown("up") || Input.GetButtonDown("down") ||
                           Input.GetButtonUp("up") || Input.GetButtonUp("down");
        
        // set velocity last change and target velocity
        if(changedDirectionX){
            velocityLastChange.x = rb.velocity.x;
            velocityChangeTimer.x = 0;
        }
        if(changedDirectionY){
            velocityLastChange.y = rb.velocity.y;
            velocityChangeTimer.y = 0;
        }

        velocityTarget = Vector2.zero;
        if(Input.GetButton("up")) velocityTarget.y += speed;
        if(Input.GetButton("down")) velocityTarget.y -= speed;
        if(Input.GetButton("left")) velocityTarget.x -= speed;
        if(Input.GetButton("right")) velocityTarget.x += speed;
    }
    void FixedUpdate(){
        velocityChangeTimer.x += Time.fixedDeltaTime;
        velocityChangeTimer.y += Time.fixedDeltaTime;
        rb.velocity = new Vector2(Mathf.Lerp(velocityLastChange.x, velocityTarget.x, Mathf.Sqrt(velocityChangeTimer.x / velocityChangeTime)),
                                  Mathf.Lerp(velocityLastChange.y, velocityTarget.y, Mathf.Sqrt(velocityChangeTimer.y / velocityChangeTime)));
    }
    void OnCollisionEnter2D(Collision2D col){
        if(col.gameObject.CompareTag("EnemyBullet")){
            hitPoints--;
            if(hitPoints <= 0) Lose();
        }
    }

    void Lose(){
        SceneManager.LoadScene("SampleScene");
    }
}
