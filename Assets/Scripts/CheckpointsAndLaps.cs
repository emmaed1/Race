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

    //[SerializeField] private List<Transform> carTransformList;

    [Header("Checkpoints")]
    private List<CheckpointSingle> checkpointSingleList;

    //private List<int> nextCheckpointSingleIndexList;
    private int nextCheckpointSingleIndex;

    private void Awake()
    {
        Transform checkpointsTransform = transform.Find("CheckPoints");

        checkpointSingleList = new List<CheckpointSingle>();
        foreach(Transform checkpointsSingle in checkpointsTransform)
        {
            CheckpointSingle checkpointSingle = checkpointsSingle.GetComponent<CheckpointSingle>();
            checkpointSingle.setTrackCheckpoints(this);
            checkpointSingleList.Add(checkpointSingle);
            Debug.Log("List: " + checkpointSingleList);
        }
        nextCheckpointSingleIndex = 0;
        //nextCheckpointSingleIndexList = new List<int>();

        //for multiplayer use?
        /*foreach(Transform carTransform in carTransformList)
        {
            nextCheckpointSingleIndexList.Add(0);
        }*/
    }

    public void CarThroughCheckpoint(CheckpointSingle checkpointSingle) 
    {   
        //int nextCheckpointSingleIndex = nextCheckpointSingleIndexList[carTransformList.IndexOf(carTransform)];
        if (checkpointSingleList.IndexOf(checkpointSingle) == nextCheckpointSingleIndex) {
            Debug.Log("Correct");

            CheckpointSingle correctSingle = checkpointSingleList[nextCheckpointSingleIndex];
            correctSingle.Hide();

            nextCheckpointSingleIndex = (nextCheckpointSingleIndex + 1) % checkpointSingleList.Count;
            //nextCheckpointSingleIndex[carTransformList.IndexOf(carTransform)] = (nextCheckpointSingleIndex + 1) % checkpointSingleList.Count;
            OnPlayerCorrectCheckpoint?.Invoke(this, EventArgs.Empty);
        } else if(checkpointSingleList.IndexOf(checkpointSingle) != nextCheckpointSingleIndex - 1)
        {
            Debug.Log(nextCheckpointSingleIndex + "Wrong");
            OnPlayerWrongCheckpoint?.Invoke(this, EventArgs.Empty);

            CheckpointSingle correctSingle = checkpointSingleList[nextCheckpointSingleIndex];
            correctSingle.Show();
        }
    }
}
