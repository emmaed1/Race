using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class CheckpointsAndLaps : NetworkBehaviour 
{
    public event EventHandler OnPlayerCorrectCheckpoint;
    public event EventHandler OnPlayerWrongCheckpoint;

    [SerializeField] private List<Transform> carTransformList;

    [Header("Checkpoints")]
    private List<CheckpointSingle> checkpointSingleList;

    private List<int> nextCheckpointSingleIndexList;

    private void Awake()
    {
        Transform checkpointsTransform = transform.Find("CheckPoints");

        checkpointSingleList = new List<CheckpointSingle>();
        foreach(Transform checkpointsSingle in checkpointsTransform)
        {
            CheckpointSingle checkpointSingle = checkpointsSingle.GetComponent<CheckpointSingle>();
            checkpointSingle.setTrackCheckpoints(this);
            checkpointSingleList.Add(checkpointSingle);
        }
        nextCheckpointSingleIndexList = new List<int>();
        foreach(Transform carTransform in carTransformList)
        {
            nextCheckpointSingleIndexList.Add(0);
        }
    }

    public void CarThroughCheckpoint(CheckpointSingle checkpointSingle, Transform carTransform) 
    {   
        int nextCheckpointSingleIndex = nextCheckpointSingleIndexList[carTransformList.IndexOf(carTransform)];
        if (checkpointSingleList.IndexOf(checkpointSingle) == nextCheckpointSingleIndex) {
            Debug.Log("Correct");

            CheckpointSingle correctSingle = checkpointSingleList[nextCheckpointSingleIndex];
            correctSingle.Hide();

            nextCheckpointSingleIndexList[carTransformList.IndexOf(carTransform)] = (nextCheckpointSingleIndex + 1) % checkpointSingleList.Count;
            OnPlayerCorrectCheckpoint?.Invoke(this, EventArgs.Empty);
        } else {
            Debug.Log(nextCheckpointSingleIndexList + "Wrong");
            OnPlayerWrongCheckpoint?.Invoke(this, EventArgs.Empty);

            CheckpointSingle correctSingle = checkpointSingleList[nextCheckpointSingleIndex];
            correctSingle.Show();
        }
    }
}
