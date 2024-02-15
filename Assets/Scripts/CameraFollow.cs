using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 offset;
    private Vector3 newPosition;
    public Transform player;

    /*void Start()
    {
        offset = player.transform.position-transform.position;
    }

    void Update()
    {
        newPosition = transform.position;
        newPosition.x = player.transform.position.x - offset.x;
        newPosition.z = player.transform.position.z - offset.z;

        transform.position = newPosition;
        //transform.position = player.transform.position-offset;
    }*/
}
