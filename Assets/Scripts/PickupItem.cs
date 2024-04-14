using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PickupItem : NetworkBehaviour
{
	[SerializeField] private GameObject PickUpPrefab;


	private void OnTriggerEnter(Collider col)
	{
		if (IsServer)
		{
			//if(col.TryGetComponent<PickUpPrefab>())
		}
	}

	public void SpawnPickUp(){
		
	}
}
