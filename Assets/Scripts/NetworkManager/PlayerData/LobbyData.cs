
using System;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyData{
    private int _mapIndex;
    private string _relayJoinCode;

    private string _senceName;


    public int MapIndex{
        get => _mapIndex;
        set => _mapIndex = value;
    }
    public string RelayCode { 
        get => _relayJoinCode;
        set => _relayJoinCode = value;
    }
    public string SenceName { 
        get => _senceName;
        set => _senceName = value;
    }

    public void Initialize(int mapIndex){
        _mapIndex = mapIndex;
    }

    public void Initialize(Dictionary<string, DataObject> lobbyData){
        UpdateState(lobbyData);
    }

    private void UpdateState(Dictionary<string, DataObject> lobbyData)
    {
        if(lobbyData.ContainsKey("MapIndex")){
            _mapIndex = Int32.Parse(lobbyData["MapIndex"].Value);
        }

        if(lobbyData.ContainsKey("RelayJoinCode")){
            _relayJoinCode = lobbyData["RelayJoinCode"].Value;
        }
        if(lobbyData.ContainsKey("SenceName")){
            _senceName = lobbyData["SenceName"].Value;
        }

    }


    public Dictionary<string, string > Serialize(){
        return new Dictionary<string, string>(){
            {"MapIndex", _mapIndex.ToString()},
            {"RelayJoinCode", _relayJoinCode},
            {"SenceName", SenceName}
        };
    }

    public void SetRelayJoinCode(string joinRelayCode){
        _relayJoinCode = joinRelayCode;
    }
    
        
}