/*
    Script to control the player character
*/

using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class CharacterController2D : MonoBehaviour
{

    //Variables to interact with the enviroment
    [SerializeField] private Transform groundCheck = null;
    [SerializeField] private LayerMask isGround;
    [SerializeField] private LayerMask isSlope;

    [SerializeField] private GameObject gameControl;

    private bool isOnGround;
    const float groundRadius = 0.2f;
    private float shotdelay = 1.5f;

    [SerializeField] private float life = 100;

    //Variables related to the movement
    private Rigidbody rb;
    private Vector3 velocity = Vector3.zero;
    [SerializeField]private bool isSliding;
    private bool jumpEnabled;
    private bool slideEnabled;
    private bool shotEnabled;
    private int dir;
    private int lastDir; 
    [Range(0, .5f)] [SerializeField] private float smooth = 0.05f;
    [SerializeField] private float jumpForce = 7000f;
    [SerializeField] private float gravity = -20f;
    [SerializeField] private float gravityFactor = 1.5f;
    [SerializeField] private float factor = 10f;
    [Range(0,1)][SerializeField] private float flySpeed = 0.4f;    
    [SerializeField] private float slideForce = 50f;
    [SerializeField] private float slideSpeed = 1.5f;
    [SerializeField] private float bulletSpeed = 25.0f;
    [SerializeField] private float damage = 10.0f;
    //Initialization of the componets
    private void Awake() {
        rb = GetComponent<Rigidbody>();
        slideEnabled = true;
        shotEnabled = true;
        lastDir = 1;
    }

    //detection of the coliders
    void FixedUpdate()
    {
        bool wasGrounded = isOnGround;
        isOnGround = false;
        jumpEnabled = false;
        //isSliding = false;

        //Check if the  player is on something that is ground to enable the jump
        Collider[] collidersGround = Physics.OverlapSphere(groundCheck.position, groundRadius, isGround);
        for (int i = 0; i < collidersGround.Length; i++){
			if (collidersGround[i].gameObject != gameObject){
				isOnGround = true;
                jumpEnabled = true;
			}
		}

        //Check if the  player is on a slope to make it lighter
        Collider[] collidersSlope = Physics.OverlapSphere(groundCheck.position, groundRadius, isSlope);
        if(collidersSlope.Length > 0){
            rb.mass = 1;
        }
        else{
            rb.mass = 30;
        }
    }

    //The basic movement of the player.
    public void Move(float moveX, float moveZ, bool jump, bool slide){
        //Indicate the direction of the player while moving
        if(moveX < 0){
            dir = -1;
            lastDir = -1;
        }
        else if(moveX > 0){
            dir = 1;
            lastDir = 1;
        }
        else
        {
            dir = 0;
        }
        Vector3 targetVelocity;

        //Jump if is the character is in the ground and the player have press the jump button 
        if(jump && jumpEnabled){
            isOnGround = false;
            jumpEnabled = false;
            rb.AddForce(new Vector3(0, jumpForce, 0));
        }

        //if is the player is on the air apply the gravity force and make it bigger when is fallin
        else if(!isOnGround){  
            if(rb.velocity.y < 0)
                rb.AddForce(new Vector3(0, gravity*gravityFactor, 0));
            else
                rb.AddForce(new Vector3(0, gravity, 0));
        }
        
        //if is possible to slide on the ground and give it an initial force to move
        //The slide mechanic only works if the player is on movement
        if(slide && slideEnabled){
            isSliding = true;
            moveX = Slide(0.8f, -90f * lastDir) * lastDir;
            moveZ = 0;
        }    

        //if the player is on the air, he has a slower movement 
        if(!isOnGround){
            moveX *= flySpeed;
            moveZ *= flySpeed;
        }

        //If the player is sliding is faster
        if(isSliding){
            moveX *= slideSpeed;
            moveZ *= slideSpeed;
        }

        moveZ = 0.0f;
        //Move the character to find the target velocity 
        targetVelocity = new Vector3(moveX * factor,  rb.velocity.y, moveZ * factor);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, smooth);
        Rotate();

    }
    //Rotate the player acording to the actions like slide or movement
    private void Rotate()
    {
        Transform t = GetComponent<Transform>();
        float ry = 0, rz = 0;
        //Debug.Log(isSliding);
        if (isSliding) rz = 90;
        else rz = 0;
        
        if (lastDir == -1) ry = 180;
        else
        {
            ry = 0;
            rz *= -1;
        }
        
        transform.rotation = Quaternion.Euler(0, ry, rz);
    }
    //Function to rotate the character and apply a force to simulate the sliding
    private float Slide(float time, float angle){
        Transform parent = transform.parent;
        //float yRotation = parent.GetComponent<Transform>().rotation.y;
        //transform.rotation  = Quaternion.Euler(0, 0, angle);  
        slideEnabled = false;
        StartCoroutine(EndSlide(time));
        return slideForce;
    }

    //Coroutine to slide for 2.5 sec and then rise again
    private IEnumerator EndSlide(float time){
        slideEnabled = false;
        yield return new WaitForSeconds(time);
        //transform.rotation  = Quaternion.Euler(0,0,0); 
        isSliding = false;
        slideEnabled = true;
        jumpEnabled = true;
    }

    public void Shoot(Transform shootOrigin, Rigidbody proyectile){
        //Create sphere
        if (shotEnabled)
        {
            Rigidbody clone = Instantiate(proyectile, shootOrigin.position, shootOrigin.rotation);
            shotEnabled = false;
            clone.velocity = new Vector3(bulletSpeed * lastDir, 0, 0);
        }
    }

    public void EnableShoot()
    {
        shotEnabled = true;
    }
    
    public float getDamage()
    {
        return damage;
    }
    
    public void Damage(float d, int index)
    {
        life -= d;
        if(life <= 0)
        {
            gameControl.GetComponent<GameControl>().GameOver(this.gameObject, index+1);
        }
    }
}
