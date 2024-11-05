

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.VisualScripting;
using UnityEngine;

public class LobbyControll : Singleton<LobbyControll>{


    public Lobby _lobby;

    public async Task<bool> CreateLobby(int maxPlayer, bool isPrivate, Dictionary<string, string> data, Dictionary<string, string> lobbydata)
    {   

        Dictionary<string, PlayerDataObject> PlayerData = SerializePlayerData(data);
        Player player = new Player(AuthenticationService.Instance.PlayerId, null, PlayerData);
        CreateLobbyOptions options = new CreateLobbyOptions{
            
            Data = SerilizeLobbyData(lobbydata),
            IsPrivate = isPrivate,
            Player = player,

        };
        try{
            _lobby = await LobbyService.Instance.CreateLobbyAsync(
            "My Lobby", maxPlayer, options
        );
        }catch(SystemException){
            return false;
        }
        
        Debug.Log($"Lobby created with lobby id {_lobby.Id} with Code {_lobby.LobbyCode}");
        
        
        
        StartCoroutine(HeartBeatLobbyCorountine(_lobby.Id, 6f));
        StartCoroutine(RefreshLobbyCorountine(_lobby.Id, 1f));
        
          
        return true;
    }

    private Dictionary<string, DataObject> SerilizeLobbyData(Dictionary<string, string> lobbydata)
    {
        Dictionary<string, DataObject> lobbyData = new Dictionary<string, DataObject>();

        foreach(var (key,value) in lobbydata ){
            lobbyData.Add(key, new DataObject(
                visibility:DataObject.VisibilityOptions.Member,
                value:value
            ));
        }
        return lobbyData;
    }

    private IEnumerator HeartBeatLobbyCorountine(string id, float v)
    {
        while(true){
            Debug.Log("HeartBeat");
            LobbyService.Instance.SendHeartbeatPingAsync(id);
            yield return new WaitForSeconds(v);

        }
    }

    private IEnumerator RefreshLobbyCorountine(string id, float v)
    {
        while(true){
            Task<Lobby> task =  LobbyService.Instance.GetLobbyAsync(id);
            yield return new WaitUntil(() => task.IsCompleted);

            Lobby newLobby = task.Result;
            if(newLobby.LastUpdated > _lobby.LastUpdated){
                _lobby = newLobby;
                LobbyEvents.OnLobbyUpdated?.Invoke(_lobby);
            }
            yield return new WaitForSeconds(v);

        }
    }

    private Dictionary<string, PlayerDataObject> SerializePlayerData(Dictionary<string, string> data)
    {
        Dictionary<string, PlayerDataObject> playerData = new Dictionary<string, PlayerDataObject>();

        foreach(var (key,value) in data ){
            playerData.Add(key, new PlayerDataObject(
                visibility:PlayerDataObject.VisibilityOptions.Member,
                value:value
            ));
        }
        return playerData;
    }

    public void OnApplicationQuit() {
        if(_lobby != null && _lobby.HostId == AuthenticationService.Instance.PlayerId){
            LobbyService.Instance.DeleteLobbyAsync(_lobby.Id);
        }
    }

    public async Task<bool> JoinLobby(string code, Dictionary<string, string> playerData)
    {
        JoinLobbyByCodeOptions options = new JoinLobbyByCodeOptions();
        Player player = new Player(AuthenticationService.Instance.PlayerId, null, SerializePlayerData(playerData));
        options.Player = player;
        try{
            _lobby = await LobbyService.Instance.JoinLobbyByCodeAsync(code, options);
        }catch(SystemException e){
            return false;  
        }
        
        StartCoroutine(RefreshLobbyCorountine(_lobby.Id, 1f));
        return true;
    }

    public List<Dictionary<string, PlayerDataObject>> GetPlayerData()
    {
        List<Dictionary<string,PlayerDataObject>> data = new List<Dictionary<string, PlayerDataObject>>();

        foreach(Player player in _lobby.Players){
            data.Add(player.Data);
        }
        return data;
    }

    public async Task<bool> UpdatePlayerData(string id, Dictionary<string, string> dictionary, string allocationID = default, string connectionData = default)
    {
        Dictionary<string, PlayerDataObject> playerData = SerializePlayerData(dictionary);
        UpdatePlayerOptions options = new UpdatePlayerOptions{
            Data = playerData,
            AllocationId = allocationID,
            ConnectionInfo = connectionData
        };
        
        try {
            _lobby = await LobbyService.Instance.UpdatePlayerAsync(_lobby.Id, id, options);
        }catch(SystemException){
            return false;
        }
       
        LobbyEvents.OnLobbyUpdated(_lobby);
        return true;

        
    }


    public async Task<bool> UpdateLobbyData(Dictionary<string, string> data)
    {
        Dictionary<string, DataObject> lobbyData = SerilizeLobbyData(data);
        UpdateLobbyOptions options = new UpdateLobbyOptions{
            Data = lobbyData
        };
        
        try {
            _lobby = await LobbyService.Instance.UpdateLobbyAsync(_lobby.Id, options);
        }catch(SystemException){
            return false;
        }
        LobbyEvents.OnLobbyUpdated(_lobby);
        return true;

        
    }

    public string GetHostID()
    {
        return _lobby.HostId;
    }
}