using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    [Range(0, .3f)] [SerializeField] private float smooth = 0.05f;
    private Rigidbody rb;
    private Vector3 velocity = Vector3.zero;

    private void Awake() {
        rb = GetComponent<Rigidbody>();

    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(float move){
        //Check if the player have control on the character
        //----

        //Move the character to find a target velocity
        Vector3 targetVelocity = new Vector2(move * 10f, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, smooth);
        
        //Flip character
        // +++++
        
    }
}
