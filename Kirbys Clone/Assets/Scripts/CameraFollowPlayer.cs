/*
    Script to make the Camera follow a target
    Requires a target (player)
    And a ofset (Vector)
    To follow the target with a smooth time
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{

    public Transform player;
    public float smoothTime = 0.5F;
    private Vector3 velocity = Vector3.zero;
    private Vector3 targetPosition;
    public Vector3 offset = new Vector3(0, 2, -5);


    void Update()
    {
        if (player)
            targetPosition = new Vector3(player.position.x, offset.y, offset.z);//player.TransformPoint(offset);
        
        // Smoothly move the camera towards that target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
