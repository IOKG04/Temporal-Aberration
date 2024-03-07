using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GunPickup : MonoBehaviour{
    public GameObject gun, interactableIndicator, takeUrGunWithYou;
    public bool active;

    void Start(){
        transform.localEulerAngles = new Vector3(0, 0, Random.Range(0, 360));
    }
    void Update(){
        if(active && Input.GetButtonDown("interact")){
            gun.SetActive(true);
            interactableIndicator.SetActive(false);
            takeUrGunWithYou.SetActive(false);
            gameObject.SetActive(false);
        }
    }
    void OnTriggerEnter2D(Collider2D col){
        if(col.CompareTag("Player")){
            active = true;
            interactableIndicator.SetActive(true);
        }
    }
    void OnTriggerExit2D(Collider2D col){
        if(col.CompareTag("Player")){
            active = false;
            interactableIndicator.SetActive(false);
        }
    }
}
