

using System.Collections.Generic;
using Unity.Services.Lobbies.Models;

public class LobbyPlayerData {
    private string _id;
    private string _gameTag;

    private bool _isReady;


    private uint _hasCodePlayer;

    private int _indexSelected = 0;

    public string Id => _id;
    public string GameTag => _gameTag;

    public uint HasCodePlayer {
        get => _hasCodePlayer;
        set => _hasCodePlayer = value;
    }

    public int IndexSelected{
        get => _indexSelected;
        set => _indexSelected = value;
    }

    public bool IsReady{
        get => _isReady;
        set => _isReady = value;
    }

    public void Initialize(string id, string gameTag){
        _id = id;
        _gameTag = gameTag;
    }

    public void Initialize(Dictionary<string, PlayerDataObject> playerData){
        UpdateState(playerData);
    }

    public void UpdateState(Dictionary<string, PlayerDataObject> playerData){
        if(playerData.ContainsKey("Id")){
            _id = playerData["Id"].Value;

        }
        if(playerData.ContainsKey("GameTag")){
            _gameTag = playerData["GameTag"].Value;
        }
        if(playerData.ContainsKey("IsReady")){
            _isReady = playerData["IsReady"].Value == "True";
        }
        if(playerData.ContainsKey("HasCodePlayer")){
            _hasCodePlayer = uint.Parse(playerData["HasCodePlayer"].Value);
        }

         if(playerData.ContainsKey("IndexSelected")){
            _indexSelected = int.Parse(playerData["IndexSelected"].Value);
        }
        

    }


    public Dictionary<string,string> Serialize(){
        return new Dictionary<string, string>(){
            {"Id",_id},
            {"GameTag", _gameTag},
            {"IsReady", _isReady.ToString()},
            {"HasCodePlayer", _hasCodePlayer.ToString()},
            {"IndexSelected", _indexSelected.ToString()}
        };

    }
}