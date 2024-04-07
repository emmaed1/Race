using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Authentication;

public class TestLobby : MonoBehaviour
{
    private Lobby hostLobby;
    private float heartbeatTimer;

    private async void Start()
    {
        await UnityServices.InitializeAsync();
        AuthenticationService.Instance.SignedIn += () =>{
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    private void Update(){
        HandleLobbyHeartbeat();
    }

    private async void HandleLobbyHeartbeat(){
        if(hostLobby != null){
            heartbeatTimer -= Time.deltaTime;
            if(heartbeatTimer < 0f){
                float heartbeatTimerMax = 15;
                heartbeatTimer = heartbeatTimerMax;

                await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
            }
        }
    }

    public async void CreateLobby(){
        try{
            string lobbyName = "My Lobby";
            int maxPlayers = 4;
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers);

            hostLobby = lobby;

            Debug.Log("Created Lobby! " + lobby.Name + " " + lobby.MaxPlayers);
            ListLobbies();
        } catch(LobbyServiceException e){
            Debug.Log(e);
        }
    }

    private async void ListLobbies(){
        try {
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions{
                Count = 25,
                Filters = new List<QueryFilter>{
                    new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT)
                },
                Order = new List <QueryOrder>{
                    new QueryOrder(false, QueryOrder.FieldOptions.Created)
                }
            };

            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync(queryLobbiesOptions);

            Debug.Log("Lobbies found: " + queryResponse.Results.Count);
            foreach(Lobby lobby in queryResponse.Results){
                Debug.Log(lobby.Name + " " + lobby.MaxPlayers);
            } 
        }
        catch(LobbyServiceException e){
            Debug.Log(e);
        }
    }

    public async void JoinLobby(){
        try {
            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();

            await Lobbies.Instance.JoinLobbyByIdAsync(queryResponse.Results[0].Id);
            Debug.Log("Lobby Joined!");
        }catch(LobbyServiceException e){
             Debug.Log(e);
        }
    }
}
