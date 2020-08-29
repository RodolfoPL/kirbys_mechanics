using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5.0f;
    public CharacterController2D controller;
    private Vector3 movement;
    private float xMovement = 0.0f; 

    //call every frame
    void Update()
    {
        //2D movement
        xMovement = Input.GetAxis("Horizontal") * speed;

    }
        
    

    private void FixedUpdate() {
        controller.Move(xMovement);
    }

}
