using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class CheckpointsAndLaps : NetworkBehaviour
{
    [Header("Checkpoints")]
    [SerializeField] public GameObject start;
    [SerializeField] public GameObject end;
    [SerializeField] public GameObject[] Checkpoints = new GameObject[0];
    [SerializeField] public Collider other;

    [Header("Settings")]
    [SerializeField] public float laps = 1;

    [Header("Information")]
    private float currCheckpoint;
    private float currLap;
    private bool started;
    private bool finished;

    private Transform checkpointsParent;

    private void Start()
    {    
        currCheckpoint = 0;
        currLap = 1;

        started = false;
        finished = false;
    }

    private void Awake()
    {
        checkpointsParent = GameObject.Find("CheckPoints").transform;
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        var startLine = Instantiate(start);
        startLine.name = start.name;
        var endLine = Instantiate(end);
        endLine.name = end.name;
        for (int i = 0; i < Checkpoints.Length; i++)
        {
            var checks = Instantiate(Checkpoints[i]);
            checks.name = Checkpoints[i].name;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Checkpoint")
        {
            Debug.Log("Here!");
            GameObject thisCheckpoint = other.gameObject;
            Debug.Log("First Checkpoint: " + thisCheckpoint);
            Debug.Log("Start: " + start);

            //started the race
            if(thisCheckpoint == start)
            {
                Debug.Log("Started!");
                started = true;
            }else if(thisCheckpoint == end && started)
            {
                //if all laps are finished, end the race
                if(currLap == laps)
                {
                    if(currCheckpoint == Checkpoints.Length)
                    {
                        finished = true;
                        Debug.Log("Finsihed!");
                    }
                    else
                    {
                        Debug.Log("Did not go through all checkpoints");
                    }
                }
                //if all laps are not finished, start a new lap
                else if(currLap < laps)
                {
                    if(currCheckpoint == Checkpoints.Length)
                    {
                        currLap++;
                        currCheckpoint = 0;
                        Debug.Log($"Started lap {currLap}");
                    }
                }
                else
                {
                    Debug.Log("Did not go through all checkpoints");
                }
            }
            //loop through checkpoints and compare and check which checkpoint the player has passed
            for(int i = 0; i < Checkpoints.Length; i++)
            {
                if(finished) return;
                if(thisCheckpoint == Checkpoints[i] && i == currCheckpoint)
                {
                    Debug.Log("Correct Checkpoint");
                    currCheckpoint++;
                }else if (thisCheckpoint == Checkpoints[i] && i != currCheckpoint)
                {
                    Debug.Log("Incorrect Checkpoint");
                }
            }
        }
    }
}
