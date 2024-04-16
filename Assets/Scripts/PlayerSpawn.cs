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
    //private GameObject mPlayer;

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
            Debug.Log("Spawn Points" + spawnPoints.Length);
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SpawnPlayer;
        }
    }

    public void SpawnPlayer(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    { 
        if (IsHost)
        {
            foreach(ulong id in clientsCompleted)
            {
                int spawn = UnityEngine.Random.Range(0, spawnPoints.Length);
                GameObject player = Instantiate(playerPrefab, spawnPoints[spawn].transform.position, Quaternion.identity);
                player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientsCompleted.First(), true);
                Debug.Log(spawnPoints[spawn].ToString());
            }   
        }
    }
}
