using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ChatUI : NetworkBehaviour
{
    [SerializeField] public Button enterBtn;
    [SerializeField] public TMP_Text inputText;
    [SerializeField] public GameObject scrollViewContent;
    [SerializeField] public GameObject messagePrefab;

    private List<ulong> dmClientIds;

    private void Start()
    {
        enterBtn.onClick.AddListener(enterEvtBtn);
    }
    private void enterEvtBtn()
    {
        //NewMessage("player2", "hello message");
        sendMessageServerRPC(inputText.text, default);
    }

    
    [ServerRpc(RequireOwnership = false)] //Server, can i post this info?
    private void sendMessageServerRPC(string message, ServerRpcParams serverRpcParams = default)
    {
        //@ClientID Message
        if (message.StartsWith("@"))
        {
            string[] parts = message.Split(' ');
            //get the clientId and remove the @
            string clientIdStr = parts[0].Replace("@", "");
            ulong toClientId = ulong.Parse(clientIdStr);

            ServerSendDirectMessage(message, serverRpcParams.Receive.SenderClientId, toClientId);
        }
        else
        {
            sendMessageClientRPC(message, serverRpcParams.Receive.SenderClientId);
        }
    }

    [ClientRpc] //fired by the server, but executed on clients
    private void sendMessageClientRPC(string message, ulong clientId)
    {
        NewMessage(message, clientId.ToString());
    }

    [ClientRpc] //fired by the server, but executed on clients
    private void recieveMessageClientRPC(string message, ulong clientId, ClientRpcParams clientRpcParams = default)
    {
        NewMessage(message, clientId.ToString());
    }

    private void ServerSendDirectMessage(string message, ulong from, ulong to)
    {
        dmClientIds[0] = from;
        dmClientIds[1] = to;

        ClientRpcParams rpcParams = default;
        
        rpcParams.Send.TargetClientIds = dmClientIds;

        recieveMessageClientRPC("<whisper> " + message.ToString(), from, rpcParams);
    }

    private void NewMessage(string message, string from)
    {
        if (message != "")
        {
            GameObject myMessage = Instantiate(messagePrefab, scrollViewContent.transform);
            myMessage.GetComponent<ChatMessageObj>().setChatMessage(from, message);
            inputText.text = "";
        }
    }
}
