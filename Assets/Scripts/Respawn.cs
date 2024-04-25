using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public PlayerSpawn spawned;
    public LayerMask Ground;
    //public GameObject player;

    public void OnCollisionExit(Collision collision)
    {
        if (collision.collider.Equals(LayerMask.NameToLayer("Ground")))
        {
            Debug.Log("You need to respawn!");
        }
    }
}
