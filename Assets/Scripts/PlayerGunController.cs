using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunController : MonoBehaviour{
    public Transform parent;
    public SpriteRenderer spriteRenderer;

    public float reloadTimer, reloadTime;
    //public float gunFlipTimer, gunFlipTime; // positive -> gun isnt flipped, negative -> gun is flipped

    public GameObject bullet;
    public float bulletSpeed;

    public TAServer server;
    private float localTimeScale;

    void Start(){
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        reloadTimer = reloadTime;
        //gunFlipTimer = gunFlipTime;
    }
    void Update(){
        // set parent rotation
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 relativePosition = mousePosition - (Vector2)parent.position;
        parent.localEulerAngles = new Vector3(0, 0, Vector2.SignedAngle(Vector2.left, relativePosition));
        //parent.up = new Vector2(relativePosition.y, -relativePosition.x);

        // set mirroring
        spriteRenderer.flipY = Vector2.Dot(parent.up, Vector2.up) < 0;
        transform.localPosition = new Vector2(transform.localPosition.x, spriteRenderer.flipY ? 0.125f : -0.125f);

        /* set gun flipping (alternative to mirroring, thats a lil more pleasent to the eye)
        NOT YET READY, ITS WEIRDLY GLITCHY
        if(Vector2.Dot(parent.up, Vector2.up) < 0){
            if(gunFlipTimer > 0f) gunFlipTimer = 0f;
            gunFlipTimer -= Time.deltaTime * localTimeScale;
            transform.localEulerAngles = new Vector3(Mathf.LerpAngle(0, 180, Mathf.Sqrt(-gunFlipTimer / gunFlipTime)), transform.localEulerAngles.y, transform.localEulerAngles.z);
        }
        else{
            if(gunFlipTimer < 0f) gunFlipTimer = 0f;
            gunFlipTimer += Time.deltaTime * localTimeScale;
            transform.localEulerAngles = new Vector3(Mathf.LerpAngle(180, 0, Mathf.Sqrt(gunFlipTimer / gunFlipTime)), transform.localEulerAngles.y, transform.localEulerAngles.z);
        }*/

        // shooting / reloading
        reloadTimer += Time.deltaTime * localTimeScale;
        if(reloadTimer > reloadTime){
            if(Input.GetButton("shoot")){
                // shoot
                GameObject newBullet = Instantiate(bullet, transform.position + (Vector3)(transform.localToWorldMatrix * new Vector3(-4.5f / 16, spriteRenderer.flipY ? -0.125f : 0.125f, 0)), Quaternion.Euler(0, 0, transform.eulerAngles.z + 90));
                newBullet.GetComponent<BulletController>().server = server;
                newBullet.GetComponent<BulletController>().velocity = newBullet.transform.up.normalized * bulletSpeed;
                reloadTimer = 0f;
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
}
