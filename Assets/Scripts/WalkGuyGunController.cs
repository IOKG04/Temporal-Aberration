using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkGuyGunController : MonoBehaviour{
    public Transform parent;
    public SpriteRenderer spriteRenderer;

    public bool active;

    public Rigidbody2D target;
    public float aimTimer, aimTime;
    private float aimTarget, lastAim;

    public float reloadTimer, reloadTime;

    public GameObject bullet;
    public float bulletSpeed;

    public TAServer server;
    private float localTimeScale;

    void Start(){
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        reloadTimer = reloadTime - aimTime;
        aimTimer = aimTime;
    }
    void Update(){
        aimTimer += Time.deltaTime * localTimeScale;
        if(active){
            // aim at player
            if(aimTimer > aimTime){
                aimTarget = Vector2.SignedAngle(Vector2.right, (Vector2)parent.position - target.position);
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
                GameObject newBullet = Instantiate(bullet, transform.position + (Vector3)(transform.localToWorldMatrix * new Vector3(-4.5f / 16, spriteRenderer.flipY ? -0.125f : 0.125f, 0)), Quaternion.Euler(0, 0, transform.eulerAngles.z + 90));
                newBullet.GetComponent<BulletController>().server = server;
                newBullet.GetComponent<BulletController>().velocity = newBullet.transform.up.normalized * bulletSpeed;
                reloadTimer = 0f;
            }
            else{
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, Mathf.Lerp(0, spriteRenderer.flipY ? -360 : 360, reloadTimer / (reloadTime - aimTime)));
            }
        }
        else{
            // aim in random direction
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
}
