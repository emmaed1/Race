using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerSpawn : NetworkBehaviour
{
    public GameObject prefabToSpawn;
    [HideInInspector] public GameObject spawnedPrefab;

    
}
