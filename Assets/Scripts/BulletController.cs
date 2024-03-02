using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour{
    public Rigidbody2D rb;
    public Vector2 velocity;

    public float lifeTimer, lifeTime;

    public TAServer server;
    private float localTimeScale;

    void Start(){
        rb = gameObject.GetComponent<Rigidbody2D>();
    }
    void FixedUpdate(){
        localTimeScale = server.LocalTimeScale(transform.position);

        rb.velocity = velocity * localTimeScale;

        lifeTimer += Time.fixedDeltaTime * localTimeScale;
        if(lifeTimer > lifeTime) Destroy(gameObject);
    }
    void OnCollisionEnter2D(Collision2D collision){
        Destroy(gameObject);
    }
}
