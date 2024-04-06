using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class CameraController : NetworkBehaviour
{
    public GameObject cameraHolder;
    public Vector3 offset;

    public override void OnNetworkSpawn()
    {
        cameraHolder.SetActive(IsOwner);
        base.OnNetworkSpawn();
    }

    private void Start(){
        if (IsOwner) {
		    this.transform.GetChild (0).gameObject.GetComponent<Camera> ().enabled = true;
		} else {
			this.transform.GetChild (0).gameObject.GetComponent<Camera> ().enabled = false;
        }
    }

    public void Update()
    {
        if (SceneManager.GetActiveScene().name == "Lobby")
        {
            cameraHolder.transform.position = transform.position + offset;
        }
    }
}
