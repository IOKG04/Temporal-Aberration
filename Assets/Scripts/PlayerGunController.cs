using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunController : MonoBehaviour{
    public Transform parent;
    public SpriteRenderer spriteRenderer;

    void Start(){
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }
    void Update(){
        // set parent rotation
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 relativePosition = mousePosition - (Vector2)parent.position;
        parent.up = new Vector2(relativePosition.y, -relativePosition.x);

        // set mirroring
        spriteRenderer.flipY = parent.rotation.eulerAngles.z > 90f && parent.rotation.eulerAngles.z <= 270f;
        transform.localPosition = new Vector2(transform.localPosition.x, spriteRenderer.flipY ? 0.15625f : -0.15625f);
    }
}
