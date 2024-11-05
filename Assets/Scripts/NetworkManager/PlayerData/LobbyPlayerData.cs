

using System.Collections.Generic;
using Unity.Services.Lobbies.Models;

public class LobbyPlayerData {
    private string _id;
    private string _gameTag;

    private bool _isReady;

    public string Id => _id;
    public string GameTag => _gameTag;

    public bool IsReady{
        get => _isReady;
        set => _isReady = value;
    }

    public void Initialize(string id, string gameTag){
        _id = id;
        _gameTag = gameTag;
    }

    public void Initialize(Dictionary<string, PlayerDataObject> playerData){
        UpdateeState(playerData);
    }

    public void UpdateeState(Dictionary<string, PlayerDataObject> playerData){
        if(playerData.ContainsKey("Id")){
            _id = playerData["Id"].Value;

        }
        if(playerData.ContainsKey("GameTag")){
            _gameTag = playerData["GameTag"].Value;
        }
        if(playerData.ContainsKey("IsReady")){
            _isReady = playerData["IsReady"].Value == "True";
        }
        

    }


    public Dictionary<string,string> Serialize(){
        return new Dictionary<string, string>(){
            {"Id",_id},
            {"GameTag", _gameTag},
            {"IsReady", _isReady.ToString()}
        };

    }
}