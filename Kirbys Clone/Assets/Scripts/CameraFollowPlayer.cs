using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{

    public Transform player;
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;

    public Vector3 offset = new Vector3(0, 2, -5);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        Vector3 targetPosition = player.TransformPoint(offset);

        // Smoothly move the camera towards that target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
