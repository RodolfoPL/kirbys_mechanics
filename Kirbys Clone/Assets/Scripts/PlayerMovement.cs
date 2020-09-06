using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float initialSpeed = 2.0f;
    public CharacterController2D controller;
    private Vector3 movement;
    private float xMovement = 0.0f; 
    private bool jump = false;

    //call every frame
    void Update()
    {
        //2D movement
        xMovement = Input.GetAxisRaw("Horizontal") * initialSpeed;
        if(Input.GetButtonDown("Jump")){
            jump = true;
        }
    }
        
    

    private void FixedUpdate() {
        controller.Move(xMovement, jump);
        jump = false;
    }

}
