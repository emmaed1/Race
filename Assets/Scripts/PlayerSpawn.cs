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

    //private GameObject mPlayer;

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
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
        if (IsHost)
        {
            foreach(ulong id in clientsCompleted)
            {
                GameObject player = Instantiate(playerPrefab);
                player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientsCompleted.First(), true);
            }
            //GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");    
        }
    }
}
