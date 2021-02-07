using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] GameObject player = null;
    private CharacterController2D cc = null;
    private float shootDelay = 1.0f;

    private float speed = 25.0f;
    private void Awake()
    {
        cc = player.GetComponent<CharacterController2D>();
        StartCoroutine("DestroyDelay");
    }

    private void OnTriggerEnter(Collider other)
    {
        string tag = other.gameObject.tag;
        if (tag == "Enemy")
        {
            other.gameObject.GetComponent<Enemy>().Hit(cc.getDamage());
            Destroy(this.gameObject);
        }
    }

    private IEnumerator DestroyDelay()
    {
        yield return new WaitForSeconds(shootDelay);
        Destroy();
    }

    private void Destroy()
    {
        cc.EnableShoot();
        //Destroy(this.gameObject); 
    }


    private void OnDestroy()
    {
        cc.EnableShoot();
    }
}
