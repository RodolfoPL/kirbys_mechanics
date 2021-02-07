/*
    Script to determine of the water is water
    or if the water is converted into ice
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    //[SerializeField] private GameObject gameControl;
    [SerializeField] private bool canKill = false;
    //State = 0 -> water
    //State = 1 -> ice
    [SerializeField] private int state = 0;

    //if the status of the water is liquid kill the player.
    private void OnTriggerEnter(Collider other) {
        string tag = other.gameObject.tag;
        if(tag == "Player" && state == 0 && canKill){
            //gameControl.GetComponent<GameControl>().GameOver(other.gameObject, 0);
            other.gameObject.GetComponent<CharacterController2D>().Damage(200, -1);
        }
        //Freeze the water
        else if(tag == "Bullet" && state == 0){
            Freeze();
            Destroy(other.gameObject);
        }
        //Melt the ice
        else if(tag == "Bullet" && state == 1){
            Melt();
            Destroy(other.gameObject);
        }
    }

    
    private void Freeze() {
        Debug.Log("Congelado");
        GetComponent<BoxCollider>().isTrigger = false;
        state = 1;
    }

    private void Melt(){
        state = 0;
        Debug.Log("Melting Ice " + this.gameObject);
        if(!canKill){
            Destroy(this.gameObject, 1.5f);
        }
        else
        {
            GetComponent<BoxCollider>().isTrigger = true;
        }
    }
}
