using Unity.Netcode;
using UnityEngine;

public class CheckpointSingle : NetworkBehaviour
{
    private CheckpointsAndLaps trackCheckpointsandLaps;
    private MeshRenderer meshRenderer;


    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        Hide();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            trackCheckpointsandLaps.CarThroughCheckpoint(this, other.transform); ;
        }
    }

    public void setTrackCheckpoints(CheckpointsAndLaps trackCheckpoints)
    {
        this.trackCheckpointsandLaps = trackCheckpoints;
    }

    public void Show()
    {
        meshRenderer.enabled = true;
    }
    public void Hide()
    {
        meshRenderer.enabled = false;
    }
}