using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunController : MonoBehaviour{
    public Transform parent;
    public SpriteRenderer spriteRenderer;

    public float reloadTimer, reloadTime;

    public GameObject bullet;
    public float bulletSpeed;

    public TAServer server;
    private float localTimeScale;
    public bool aimable;

    void Start(){
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        reloadTimer = reloadTime;
    }
    void Update(){
        if(aimable){
            // set parent rotation
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 relativePosition = mousePosition - (Vector2)parent.position;
            parent.localEulerAngles = new Vector3(0, 0, Vector2.SignedAngle(Vector2.left, relativePosition));

            // set mirroring
            spriteRenderer.flipY = Vector2.Dot(parent.up, Vector2.up) < 0;
            transform.localPosition = new Vector2(transform.localPosition.x, spriteRenderer.flipY ? 0.125f : -0.125f);
        }

        // shooting / reloading
        reloadTimer += Time.deltaTime * localTimeScale;
        if(reloadTimer > reloadTime){
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0f);
            if(Input.GetButton("shoot") && CanSeeGunBarrel()){
                // shoot
                GameObject newBullet = Instantiate(bullet, transform.position + (Vector3)(transform.localToWorldMatrix * new Vector3(-0.28125f, spriteRenderer.flipY ? -0.125f : 0.125f, 0)), Quaternion.Euler(0, 0, transform.eulerAngles.z + 90));
                newBullet.GetComponent<BulletController>().server = server;
                newBullet.GetComponent<BulletController>().velocity = newBullet.transform.up.normalized * bulletSpeed;
                reloadTimer = 0f;
                // screen shake
                Camera.main.GetComponent<CameraController>().ShakeScreen(0.25f, 0.13f);
            }
        }
        else{
            // reload animation
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, Mathf.Lerp(0, spriteRenderer.flipY ? -360 : 360, reloadTimer / reloadTime));
        }
    }
    void FixedUpdate(){
        localTimeScale = server.LocalTimeScale(transform.position);
    }

    bool CanSeeGunBarrel(){
        Vector2 target = transform.position + (Vector3)(transform.localToWorldMatrix * new Vector3(-0.28125f, spriteRenderer.flipY ? -0.125f : 0.125f, 0));
        RaycastHit2D[] hits = Physics2D.RaycastAll(parent.position, target - (Vector2)parent.position, 1f);
        for(int i = 1; i < hits.Length; i++){
            if(hits[i].collider.CompareTag("Wall")) return false;
        }
        return true;
    }
}
