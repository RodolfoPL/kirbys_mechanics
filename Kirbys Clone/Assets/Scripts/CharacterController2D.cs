using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class CharacterController2D : MonoBehaviour
{
    [Range(0, .5f)] [SerializeField] private float smooth = 0.05f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform ceilCheck;
    [SerializeField] private float jumpForce = 7000f;
    [SerializeField] private float gravity = -20f;
    [SerializeField] private LayerMask isGround;
    [SerializeField] private LayerMask isSlope;

    [SerializeField] private float factor = 10f;
    [Range(0,1)][SerializeField] private float crouchSpeed = 0.8f;
    [Range(0,1)][SerializeField] private float flySpeed = 0.4f;
    
    [SerializeField] private Collider characterCollider;
    [SerializeField] private float gravityFactor = 1.5f;

    private Rigidbody rb;
    private Transform tr;
    private bool isOnGround;    
    private Vector3 velocity = Vector3.zero;
    [SerializeField] private bool jumpEnabled;

    [SerializeField] private bool slideEnabled;
    [SerializeField] private float smoothSlide = 0.5f;
    [SerializeField] private float slideForce = 50f;

   // [SerializeField] private float culdown = 1f;

    const float groundRadius = 0.2f;
    const float ceilRadius = 0.2f;


    private void Awake() {
        rb = GetComponent<Rigidbody>(); 
        tr = GetComponent<Transform>();
        slideEnabled = true;
    }


    void FixedUpdate()
    {
        bool wasGrounded = isOnGround;
        isOnGround = false;
        jumpEnabled = false;
        //isCrouch = false;
        
        //Check if the  player is on something that is ground
        Collider[] collidersGround = Physics.OverlapSphere(groundCheck.position, groundRadius, isGround);
        for (int i = 0; i < collidersGround.Length; i++){
           // Debug.Log(collidersGround[i]);
			if (collidersGround[i].gameObject != gameObject){
				isOnGround = true;
                jumpEnabled = true;
       /*         Vector3 ground = collidersGround[i].gameObject.transform.position;
                float dotProduct = Vector3.Dot(ground.normalized, transform.position.normalized);
                Debug.Log(dotProduct);
                if(dotProduct == 0.0f){
                    Debug.Log("Perpendicular");
                }*/
			}
		}

        //Check if the  player is on something that is ground
        Collider[] collidersSlope = Physics.OverlapSphere(groundCheck.position, groundRadius, isSlope);
        if(collidersSlope.Length > 0){
            Debug.Log("Slide weee!");
            rb.mass = 1;
           // Slide(2.5f, 90f);
        }
        else{
            rb.mass = 30;
        }
    }



    public void Move(float moveX, float moveZ, bool jump, bool slide /*bool crouch*/){
        int dir;
        if(moveX < 0){
            dir = -1;
        }
        else if(moveX > 0)
            dir = 1;
        else
        {
            dir = 0;
        }
        Vector3 targetVelocity;
       // Vector3 move = new Vector3(moveX, 0, moveZ);
        //Jump if is the character is in the ground and the player press the jump button 
        if(jump && jumpEnabled){
            isOnGround = false;
            jumpEnabled = false;
            rb.AddForce(new Vector3(0, jumpForce, 0));
        }//if is flying only gravity is a force
        else if(!isOnGround){  
            //the gravity is stronger if the player has reach the max height
            if(rb.velocity.y < 0)
                rb.AddForce(new Vector3(0, gravity*gravityFactor, 0));
            else
                rb.AddForce(new Vector3(0, gravity, 0));
        }
        
        //if is possible to slide
        if(slide && slideEnabled){
            moveX = Slide(0.8f, -90f * dir) * dir;
            moveZ = 0;
        }    

        //check if can stand up
     /*   if(!crouch){
            Collider[] collidersGroundTop = Physics.OverlapSphere(ceilCheck.position, ceilRadius, isGround);
            //if not stay crouch
            //Debug.Log(collidersGroundTop.Length);
            if(collidersGroundTop.Length > 0){
                crouch = true;
            }
        }
        //when is crouch modify the colider and the speed
        if(crouch && isOnGround){
            //isCrouch = true;
            //targetVelocity = new Vector2(move * (factor * 0.5f), rb.velocity.y);
            moveX *= crouchSpeed;
            moveZ *= crouchSpeed;
            if(characterCollider != null) characterCollider.enabled = false;
        }else if(crouch && !isOnGround){
            moveX *= flySpeed;
            moveZ *= flySpeed;
        }
        else{
            if(characterCollider != null) characterCollider.enabled = true;
        }*/

        //if the player is on the air, he has a slower movement 
        if(!isOnGround){
            moveX *= flySpeed;
            moveZ *= flySpeed;
        }
        
        //Move the character to find the target velocity 
        targetVelocity = new Vector3(moveX * factor,  rb.velocity.y, moveZ * factor);
        
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, smooth);
                 
        //Flip character
        
    }

    private float Slide(float time, float angle){
        Transform parent = transform.parent;
        //rotate the character
        transform.rotation  = Quaternion.Euler(0,0,angle);  
        slideEnabled = false;
        StartCoroutine(EndSlide(time));
        //move the character forward
        return slideForce;
    }

    //Coroutine to slide for 2.5 sec and then rise again
    private IEnumerator EndSlide(float time){
        slideEnabled = false;
        yield return new WaitForSeconds(time);
        transform.rotation  = Quaternion.Euler(0,0,0); 
        slideEnabled = true;
    }
}
