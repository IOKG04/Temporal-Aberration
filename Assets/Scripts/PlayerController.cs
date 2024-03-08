using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour{
    public Rigidbody2D rb;

    public Vector2 velocityTarget, velocityLastChange, velocityChangeTimer;
    public float velocityChangeTime;
    public float speed, walkSpeed, runSpeed;
    private bool changedDirectionX, changedDirectionY; // was a direction button pressed this physics frame
    public PlayerGunController gun;

    public int hitPoints, maxHitPoints;
    public VoidTask hitPointReduction;
    public SpriteRenderer spriteRenderer;
    public Sprite[] sprites;

    void Start(){
        rb = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        hitPoints = maxHitPoints;
        hitPointReduction += () => {
            // change sprite
            if(hitPoints < sprites.Length){
                spriteRenderer.sprite = sprites[hitPoints];
            }
            // lose is too few hitpoints
            if(hitPoints <= 0){
                Lose();
            }
            // screen shake
            Camera.main.GetComponent<CameraController>().ShakeScreen(0.5f, Mathf.Lerp(0.2f, 0.35f, 1 - hitPoints / (float)maxHitPoints));
        };
        // set TAServer stuff
        try{
            TAServer server = gameObject.GetComponent<TAServer>();
            server.distanceScale = Metadata.LevelNumber * Metadata.LevelNumber / 8f;
            server.timeScale = 0.1f * Metadata.LevelNumber;
        }
        catch{ Debug.LogWarning("No TAServer found on player"); }
    }

    void Update(){
        // set speed
        speed = walkSpeed;
        gun.aimable = true;
        if(Input.GetButton("sprint")){
            speed = runSpeed;
            gun.aimable = false;
        }

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
            hitPointReduction();
        }
    }

    void Lose(){
        SceneManager.LoadScene(SceneManager.GetSceneAt(0).name, LoadSceneMode.Single);
    }
}

public delegate void VoidTask();