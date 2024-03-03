using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class WalkGuyGunController : MonoBehaviour{
    public Transform parent;
    public SpriteRenderer spriteRenderer;

    public bool active;

    public Rigidbody2D target;
    public float aimTimer, aimTime, aimDistance, aimPredictionChance;
    private float aimTarget, lastAim;

    public float reloadTimer, reloadTime;
    private bool showReloadAnimation;

    public GameObject bullet;
    public float bulletSpeed;

    public TAServer server;
    private float localTimeScale;

    void Start(){
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        reloadTimer = 0f;
        showReloadAnimation = false;
        active = false;
        aimTime *= Random.Range(0.9f, 1.1f);
        reloadTime *= Random.Range(0.9f, 1.1f);
        bulletSpeed *= Random.Range(0.95f, 1.05f);
        aimPredictionChance *= Random.Range(0.9f, 1.1f);
        aimTimer = aimTime;
    }
    void Update(){
        if(active){
            // aim at player
            aimTimer += Time.deltaTime * localTimeScale;
            if(aimTimer > aimTime){
                if(Random.Range(0, 1) < aimPredictionChance){
                    Vector2 predictedPosition = target.position + (target.velocity * ((target.position - (Vector2)transform.position).magnitude / bulletSpeed) * Random.Range(0.9f, 1.05f));
                    aimTarget = Vector2.SignedAngle(Vector2.right, (Vector2)parent.position - predictedPosition);
                }
                else aimTarget = Vector2.SignedAngle(Vector2.right, (Vector2)parent.position - target.position);
                lastAim = parent.localEulerAngles.z;
                aimTimer = 0f;
            }
            parent.localEulerAngles = new Vector3(0, 0, Mathf.LerpAngle(lastAim, aimTarget, aimTimer / aimTime));

            // set mirroring
            spriteRenderer.flipY = Vector2.Dot(parent.up, Vector2.up) < 0;
            transform.localPosition = new Vector2(transform.localPosition.x, spriteRenderer.flipY ? 0.125f : -0.125f);

            // shooting / reloading
            reloadTimer += Time.deltaTime * localTimeScale;
            if(reloadTimer > reloadTime){
                showReloadAnimation = false;
                if(CanSeeTarget()){
                    GameObject newBullet = Instantiate(bullet, transform.position + (Vector3)(transform.localToWorldMatrix * new Vector3(-4.5f / 16, spriteRenderer.flipY ? -0.125f : 0.125f, 0)), Quaternion.Euler(0, 0, transform.eulerAngles.z + 90));
                    newBullet.GetComponent<BulletController>().server = server;
                    newBullet.GetComponent<BulletController>().velocity = newBullet.transform.up.normalized * bulletSpeed;
                    newBullet.tag = "EnemyBullet";
                    showReloadAnimation = true;
                }
                reloadTimer = 0f;
            }
            else if(showReloadAnimation){
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, Mathf.Lerp(0, spriteRenderer.flipY ? -360 : 360, reloadTimer / (reloadTime - aimTime)));
            }
        }
        else{
            // aim in random direction
            aimTimer += Time.deltaTime * localTimeScale;
            if(aimTimer > aimTime * 8){
                aimTarget = Random.Range(0, 360);
                lastAim = parent.localEulerAngles.z;
                aimTimer = 0f;
            }
            parent.localEulerAngles = new Vector3(0, 0, Mathf.LerpAngle(lastAim, aimTarget, aimTimer / (aimTime * 6)));

            // set mirroring
            spriteRenderer.flipY = Vector2.Dot(parent.up, Vector2.up) < 0;
            transform.localPosition = new Vector2(transform.localPosition.x, spriteRenderer.flipY ? 0.125f : -0.125f);
        }
    }
    void FixedUpdate(){
        localTimeScale = server.LocalTimeScale(transform.position);
    }
    bool CanSeeTarget(){
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, target.position - (Vector2)transform.position, aimDistance);
        for(int i = 1; i < hits.Length; i++){
            if(!hits[i].collider.isTrigger && hits[i].collider.gameObject.CompareTag("WalkGuy")) return false;
        }
        return true;
    }
}