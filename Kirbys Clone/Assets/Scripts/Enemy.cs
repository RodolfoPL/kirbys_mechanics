using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody rb;
    private float InitialPosition;
    private int direction = -1;

    private bool isFreeze = false;
    private bool stop = false;
   // private float freezeTime = 5.0f;
    [SerializeField] private BoxCollider coll;

    [SerializeField] private float life = 20.0f;


    //Movement Variables
    [SerializeField] private float speed = 10f;
    [Range(0,1)][SerializeField] private float smooth = .5f;
    [SerializeField] private float[] patrolArea = new float[2];
    /*
     * 0 = Robot
     * 1 = animal
    */
    [SerializeField] private float[] freezeTime = new float[] { 0.2f, 5.0f };
    [SerializeField] int type;
    [SerializeField] private float[] power = new float[] { 30.0f, 20.0f };

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        InitialPosition = GetComponent<Transform>().position.x;
        //Debug.Log(InitialPosition);
    }

    private void Update()
    {
        if (!isFreeze && !stop)
        {
            Move();
        }
    }

    //Move the enemy left ro right
    private void Move()
    {
        Vector3  targetVelocity = new Vector3(speed * direction, 0, 0);
        Vector3 velocity = Vector3.zero;
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, smooth);
        Turn();
    }

    //Turn the Enemy if Needed
    private void Turn()
    {
        float xPosition = GetComponent<Transform>().position.x;
        if(xPosition <= (InitialPosition - patrolArea[0]) && direction == -1) //Min Left direction
        {
            direction = 1;
            GetComponent<Transform>().rotation = Quaternion.Euler(0, 180, 0);
        }
        else if(xPosition >= (InitialPosition + patrolArea[1]) && direction == 1)
        {
            direction = -1;
            GetComponent<Transform>().rotation = Quaternion.Euler(0, 0, 0);
        }
        
    }

    public void Hit(float damage)
    {
        isFreeze = true;
        StartCoroutine(UnFreaze());

        if (type == 0)
        {
            Damage(damage);
            
        }
        else if(type == 1)
        {
            rb.useGravity = false;
            coll.enabled = false;
        }
    }

    private void Damage(float damage)
    {
        life -= damage;
        //the enemy is defeat
        if(life <= 0.0f)
        {
            Destroy(this.gameObject);
            //Destroy effects
        }
    }

    private IEnumerator UnFreaze()
    {
        yield return new WaitForSeconds(freezeTime[type]);
        isFreeze = false;
        coll.enabled = true;
        rb.useGravity = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<CharacterController2D>().Damage(power[type], type);
            stop = true;
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.5f);
        stop = false;
    }
}
