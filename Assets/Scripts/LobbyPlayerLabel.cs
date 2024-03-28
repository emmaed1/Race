using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPlayerLabel : MonoBehaviour
{
    [SerializeField] TMP_Text PlayerText;
    [SerializeField] Image readyImage, colorImage;
    [SerializeField] Button kickBtn;

    public event Action<ulong> onKickClicked;
    private ulong clientId;

    public void OnEnable()
    {
        kickBtn.onClick.AddListener(BtnKick_Clicked);
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

    public void setKickActive(bool isOn)
    {
        kickBtn.gameObject.SetActive(isOn);
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
}
