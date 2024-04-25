using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerSpawn : NetworkBehaviour
{
    [SerializeField] public GameObject playerPrefab;
    [SerializeField] public GameObject[] spawnPoints;
    private List<GameObject> spawnedPoints;
    NetworkVariable<int> spawn = new NetworkVariable<int>(0);


    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Awake()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (base.IsHost)
        {
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SpawnPlayer;
        }
    }

    public void SpawnPlayer(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    { 
        if (IsServer)
        {
            foreach (ulong id in clientsCompleted)
            {
                GameObject player = Instantiate(playerPrefab, spawnPoints[spawn.Value].transform.position, Quaternion.identity);
                player.GetComponent<NetworkObject>().SpawnAsPlayerObject(id, true);
                player.name = playerPrefab.name;
                spawn.Value++;
            }
        }
    }
}
