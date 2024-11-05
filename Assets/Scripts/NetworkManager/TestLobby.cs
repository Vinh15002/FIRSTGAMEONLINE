

using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services;
using UnityEngine;
using Unity.Services.Lobbies.Models;
using System.Collections.Generic;
using System;

public class TestLobby : MonoBehaviour
{

    public String lobbyCode;
    private Lobby hostLobby;
    private float heartbeatTimer;

    private string playerName;
    private async void Start(){
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () => {
            Debug.Log("Signed in: " + AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        playerName = "VinhTran" + UnityEngine.Random.Range(10,100);
        Debug.Log(playerName);
    }

    private void Update(){
        HandleLobbyHeartBeat();
    }
    private async void HandleLobbyHeartBeat(){
        if(hostLobby != null){
            heartbeatTimer -= Time.deltaTime;
            if(heartbeatTimer < 0f){
                float heartbeatTimerMax = 15f;
                heartbeatTimer = heartbeatTimerMax;

                await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
            }
        }
    }


    [ContextMenu("CreateLobby")]
    private async void CreateLobby(){
        try{
            CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions{
                IsPrivate = false,
                Player = GetPlayer(),
            };
            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(
                "MyLobby", 4, createLobbyOptions
            );
            Debug.Log("Created Lobby! " + lobby.Name + " " + lobby.MaxPlayers + " " + lobby.Id + " " + lobby.LobbyCode);
            hostLobby = lobby;
            PrintPlayers(hostLobby);
           
        }catch(LobbyServiceException e){
            Debug.Log(e);
        }
    }


    [ContextMenu("ListLobby")]
    private async void ListLobbies(){
        try{

            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions{
                Count = 25,
                Filters = new List<QueryFilter> {
                    new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT)
                },

                Order = new List<QueryOrder> {
                    new QueryOrder(false, QueryOrder.FieldOptions.Created)
                }
            };

            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync(queryLobbiesOptions);

            Debug.Log("Lobbies found: " + queryResponse.Results.Count);

            foreach(Lobby lobby in queryResponse.Results){
                Debug.Log(lobby.Name + " " + lobby.MaxPlayers);
            }


        }catch(LobbyServiceException e){
            Debug.Log(e);
        }
    }



    [ContextMenu("JoinLobbyByCode")]

    private async void JoinLobbyByCode(){
        try {
            
            JoinLobbyByCodeOptions joinLobbyByCodeOptions = new JoinLobbyByCodeOptions{
                Player = GetPlayer(),
            };
            Debug.Log(this.lobbyCode);
            Lobby joinedLobby = await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode, joinLobbyByCodeOptions);
            PrintPlayers(joinedLobby);
        }catch (LobbyServiceException e){
            Debug.Log(e);
        }
    }


    [ContextMenu("Seeplayer")]

    private  void SeePlayer(){
        try {
           
            PrintPlayers(hostLobby);
        }catch (LobbyServiceException e){
            Debug.Log(e);
        }
    }


    
    private async void QuickJoinLobby(){
        try{
            await Lobbies.Instance.QuickJoinLobbyAsync();
        }catch (LobbyServiceException e){
            Debug.Log(e);
        }
    }


    private void PrintPlayers(Lobby lobby){
        Debug.Log("Player in Lobby: " + lobby.Name);
        foreach(Player player in lobby.Players){
            Debug.Log(player.Id + " " + player.Data["playerName"].Value);
        }
    }


    public void onSetLobbyCode(String code){
        this.lobbyCode = code;
        
    }



    private Player GetPlayer(){

        return new Player{
             Data = new Dictionary<string, PlayerDataObject>{
                        {"playerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerName )}
                    }
        };
    }











}
