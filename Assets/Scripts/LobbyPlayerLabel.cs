using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPlayerLabel : MonoBehaviour
{
    [SerializeField] TMP_Text PlayerText;
    [SerializeField] protected Image readyImage, colorImage;
    [SerializeField] protected Button Kick_Btn;

    public event Action<ulong> onKickClicked;
    private ulong clientId;

    public void OnEnable()
    {
        Kick_Btn.onClick.AddListener(BtnKick_Clicked);
    }

    public void setPlayerName(ulong playerName)
    {
        clientId = playerName;
        PlayerText.text = "Player " + playerName.ToString();
    }

    private void BtnKick_Clicked()
    {
        onKickClicked?.Invoke(clientId);
    }

    public void SetReady(bool ready)
    {
        if (ready)
        {
            readyImage.material.color = Color.green;
        }
        else
        {
            readyImage.material.color = Color.red;
        }
    }

    public void SetIconColor(Color color)
    {
        colorImage.material.color = color;
    }

    public void enableKick(bool enabled)
    {
        Kick_Btn.gameObject.SetActive(enabled);
    }
}
