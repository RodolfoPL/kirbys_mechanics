using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class CharacterController2D : MonoBehaviour
{
    [Range(0, .5f)] [SerializeField] private float smooth = 0.05f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform ceilCheck;
    [SerializeField] private float maxJump = 7000f;
    [SerializeField] private float jumpForce = 7000f;
    [SerializeField] private float gravity = -20f;
    [SerializeField] private LayerMask isGround;
    [SerializeField] private float factor = 10f;
    [Range(0,1)][SerializeField] private float crouchSpeed = 0.8f;
    [Range(0,1)][SerializeField] private float flySpeed = 0.4f;
    
    [SerializeField] private Collider characterCollider;
    
    private Rigidbody rb;
    private bool isOnGround;    
    private Vector3 velocity = Vector3.zero;
    [SerializeField] private bool jumpEnabled;
    private bool isCrouch;

    [SerializeField] private float culdown = 1f;

    const float groundRadius = 0.2f;
    const float ceilRadius = 0.2f;


    private void Awake() {
        rb = GetComponent<Rigidbody>(); 
    }


    void FixedUpdate()
    {
        bool wasGrounded = isOnGround;
        isOnGround = false;
       /* jumpEnabled = false;
        isCrouch = false;
        */

        //Check if the  player is on something that is ground
        Collider[] colliders = Physics.OverlapSphere(groundCheck.position, groundRadius, isGround);
        for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				isOnGround = true;
                jumpEnabled = true;
                jumpForce = maxJump;
			}
		}
    }

    public void Move(float move, bool jump, bool crouch){
        Vector3 targetVelocity;

        //Jump if is the character is in the ground and the player press the jump button 
        if(jump && jumpEnabled){

            //Slowly decrece Jump Force
            if(jumpForce > 2000f){
                jumpForce *= .8f;
            }
            else{
                jumpForce = 2000f;
            }
            print(jumpForce);

            isOnGround = false;
            jumpEnabled = false;
            rb.AddForce(new Vector3(0, jumpForce, 0));
            StartCoroutine(Jump());
        }//if is flying only gravity is a force
        else if(!isOnGround){
            rb.AddForce(new Vector3(0, gravity, 0));
        }
        

        //check if can stand up
        if(!crouch){
            Collider[] collidersTop = Physics.OverlapSphere(ceilCheck.position, ceilRadius, isGround);
            //if not stay crouch
            //Debug.Log(collidersTop.Length);
            if(collidersTop.Length > 0){
                crouch = true;
            }
        }
        //when is crouch modify the colider and the speed
        if(crouch && isOnGround){
            isCrouch = true;
            //targetVelocity = new Vector2(move * (factor * 0.5f), rb.velocity.y);
            move *= crouchSpeed;
            if(characterCollider != null) characterCollider.enabled = false;
        }else if(crouch && !isOnGround){
            move *= 0.9f;
        }
        else{
            if(characterCollider != null) characterCollider.enabled = true;
        }

        //if the player is on the air, he has a slower movement 
        if(!isOnGround){
            move *= flySpeed;
        }
        
        //Move the character to find the target velocity 
        targetVelocity = new Vector2(move * factor, rb.velocity.y);
        
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, smooth);

          
        
        //Flip character
        // +++++
        
    }

    IEnumerator Jump()
    {
        //print(Time.time);
        yield return new WaitForSecondsRealtime(culdown);
        jumpEnabled = true;
        //print(Time.time);
    }

}
