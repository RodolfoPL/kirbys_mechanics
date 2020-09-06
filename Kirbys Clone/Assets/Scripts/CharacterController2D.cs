using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
    [Range(0, .5f)] [SerializeField] private float smooth = 0.05f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float jumpForce = 40f;
    [SerializeField] private LayerMask isGround;
    private Rigidbody rb;
    public bool isOnGround;    
    private Vector3 velocity = Vector3.zero;

    const float groundRadius = 0.2f;


    private void Awake() {
        rb = GetComponent<Rigidbody>();   
    }


    void FixedUpdate()
    {
        bool wasGrounded = isOnGround;
        isOnGround = false;

        //Check if the  player is on something that is ground
        Collider[] colliders = Physics.OverlapSphere(groundCheck.position, groundRadius, isGround);
        for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				isOnGround = true;
			}
		}
    }

    public void Move(float move, bool jump){
        //Check if the player have control on the character
        //----


        //Jump if is the character is in the ground and the player press the jump button
        if(jump){
            isOnGround = false;
            rb.AddForce(new Vector3(0, jumpForce, 0));
        }

        //Move the character to find a target velocity
        Vector3 targetVelocity = new Vector2(move * 10f, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, smooth);

        
        
        //Flip character
        // +++++
        
    }
}
