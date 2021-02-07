/*
    Class to detect the imput of the player and move the character
*/
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float initialSpeed = 2.0f;
    [SerializeField] private CharacterController2D controller;
    [SerializeField] private Transform shootOrigin;
    [SerializeField] private Rigidbody proyectile;

    private Vector3 movement;
    private float xMovement = 0.0f, zMovement = 0.0f; 
    private bool jump = false;
    private bool slide = false;

    void Update()
    {
        xMovement = Input.GetAxisRaw("Horizontal") * initialSpeed * Time.deltaTime;
        zMovement = Input.GetAxisRaw("Vertical") * initialSpeed * Time.deltaTime;
        if(Input.GetButtonDown("Jump")){
            jump = true;
        }
        if(Input.GetButtonDown("Fire2")) 
            slide = true;

        if(Input.GetButtonDown("Fire1")){
            controller.Shoot(shootOrigin, proyectile);
        }
    }
        
    

    private void FixedUpdate() {
        controller.Move(xMovement, zMovement, jump, slide/*, crouch*/);
        slide = false;
        jump = false;
        
        
    }

}
