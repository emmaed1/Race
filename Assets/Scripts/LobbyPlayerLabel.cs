using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPlayerLabel : NetworkBehaviour
{
    [SerializeField] TMP_Text PlayerText;
    [SerializeField] protected Image readyImage, colorImage;
    public void setPlayerName(string playerName)
    {
        PlayerText.text = "Player " + playerName;
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
