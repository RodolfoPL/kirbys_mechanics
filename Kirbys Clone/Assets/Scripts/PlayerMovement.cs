using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float initialSpeed = 2.0f;
    public CharacterController2D controller;
    private Vector3 movement;
    private float xMovement = 0.0f, zMovement = 0.0f; 
    private bool jump = false;
   // private bool crouch = false;
    private bool slide = false;

    //call every frame
    void Update()
    {
        //2D movement
        xMovement = Input.GetAxisRaw("Horizontal") * initialSpeed * Time.deltaTime;
        zMovement = Input.GetAxisRaw("Vertical") * initialSpeed * Time.deltaTime;
        if(Input.GetButtonDown("Jump")){
            jump = true;
        }
        /*if(Input.GetButtonDown("Crouch")){
            crouch =  !crouch;
        }*/
        if(Input.GetKeyDown("e")) slide = true;
    }
        
    

    private void FixedUpdate() {
        controller.Move(xMovement, zMovement, jump, slide/*, crouch*/);
        slide = false;
        jump = false;
        
    }

}
