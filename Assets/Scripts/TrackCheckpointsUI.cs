using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TrackCheckpointsUI : NetworkBehaviour
{
    [SerializeField] private CheckpointsAndLaps trackCheckpointsAndLaps;

    private void Start()
    {
        trackCheckpointsAndLaps.OnPlayerCorrectCheckpoint += CheckpointsAndLaps_OnplayerCorrectCheckpoint;
        trackCheckpointsAndLaps.OnPlayerWrongCheckpoint += CheckpointsAndLaps_OnPlayerWrongCheckpoint;

        Hide();
    }

    private void CheckpointsAndLaps_OnPlayerWrongCheckpoint(object sender, EventArgs e)
    {
        Show();
    }

    private void CheckpointsAndLaps_OnplayerCorrectCheckpoint(object sender, EventArgs e)
    {
        Hide();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
