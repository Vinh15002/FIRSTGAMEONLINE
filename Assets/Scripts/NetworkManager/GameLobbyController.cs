using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLobbyController: Singleton<GameLobbyController>
{
    private List<LobbyPlayerData> _lobbyPlayerDatas = new List<LobbyPlayerData>();
    private LobbyPlayerData _localPlayerData;


    public bool IsHost {
        get {return _localPlayerData.Id == LobbyControll.Instance.GetHostID();}
    }
    private LobbyData _lobbyData;

    public Lobby _lobby;
    public Coroutine _heartbeat;
    public Coroutine _refresh;

    private bool loaded = true;

    public string GetLobbyCode()
    {
        return LobbyControll.Instance._lobby.LobbyCode;
    }

    public async Task<bool> CreateLobby()
    {
        _localPlayerData = new LobbyPlayerData();
        _localPlayerData.Initialize(AuthenticationService.Instance.PlayerId, "HostPlayer");
        _lobbyData = new LobbyData();
        _lobbyData.MapIndex = 0;
        _lobbyData.SenceName = "MainSence";
        _lobbyData.Initialize(0);
        await LobbyControll.Instance.CreateLobby(4, false, _localPlayerData.Serialize(), _lobbyData.Serialize());


        return true;
    }

    public void Test(){
        Debug.Log("Hello");
    }

    public async Task<bool> JoinLobby(string code)
    {
        _localPlayerData = new LobbyPlayerData();
        _localPlayerData.Initialize(AuthenticationService.Instance.PlayerId, "JoinPlayer");
        
        bool suc = await LobbyControll.Instance.JoinLobby(code, _localPlayerData.Serialize());
        return suc;

    }

    private void OnEnable() {
        LobbyEvents.OnLobbyUpdated += LobbyUpdated;
    }
    private void OnDisable() {
        LobbyEvents.OnLobbyUpdated -= LobbyUpdated;
    }
       

    private async void LobbyUpdated(Lobby lobby)
    {
       
       


        List<Dictionary<string,PlayerDataObject>> playerData = LobbyControll.Instance.GetPlayerData();
        _lobbyPlayerDatas.Clear();

        int count =  0;
        foreach(var data in playerData){
            LobbyPlayerData lobbyPlayerData = new LobbyPlayerData();
            lobbyPlayerData.Initialize(data);
            if(lobbyPlayerData.IsReady){
                count ++;
            }
            _lobbyPlayerDatas.Add(lobbyPlayerData);
           
            if(lobbyPlayerData.Id == AuthenticationService.Instance.PlayerId){
                _localPlayerData = lobbyPlayerData;
                
            }
        }

        _lobbyData = new LobbyData();
        _lobbyData.Initialize(lobby.Data);

        LobbyEvents.lobbyUpdateUI?.Invoke();

        
        LobbyUI.instance.SetMap(_lobbyData.MapIndex);
        if(count == _lobbyPlayerDatas.Count){
            LobbyUI.instance.SetStartButton();

           
        }

        if(_lobbyData.RelayCode != default && _localPlayerData.Id != LobbyControll.Instance.GetHostID() && loaded ){
            loaded = false;
            await SceneManager.LoadSceneAsync(_lobbyData.SenceName);
            
            JoinAllocation joinAllocation = await JoinRelaySercer(_lobbyData.RelayCode);
            

            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls"); 
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartClient();
        }
      

        
    
    }

    private async Task<JoinAllocation> JoinRelaySercer(string relayCode)
    {
        JoinAllocation joinAllocation = await RelayManager.Instance.JoinRelay(relayCode);
        
        string allocationID = RelayManager.Instance.GetAllocation();
        string connectionData = RelayManager.Instance.GetConnectionData();
        await LobbyControll.Instance.UpdatePlayerData(_localPlayerData.Id, _localPlayerData.Serialize(), allocationID, connectionData);

        return joinAllocation;
    }

    public List<LobbyPlayerData> GetPlayers()
    {
        return _lobbyPlayerDatas;
    }

    public async Task<bool> SetPlayerRead()
    {
        _localPlayerData.IsReady = true;
        return await LobbyControll.Instance.UpdatePlayerData(_localPlayerData.Id, _localPlayerData.Serialize());
    }

    public async Task<bool> SetSelectedMap(int _currentMapIndex, string senceName)
    {
        _lobbyData.MapIndex = _currentMapIndex;
        _lobbyData.SenceName = senceName;
        Debug.Log("Sence: " + senceName);
        return await LobbyControll.Instance.UpdateLobbyData(_lobbyData.Serialize());
    }

    public async Task StartGame()
    {
        Allocation hostAllocation = await RelayManager.Instance.CreateRelay();

        _lobbyData.SetRelayJoinCode(RelayManager.Instance._joinCode);


        await LobbyControll.Instance.UpdateLobbyData(_lobbyData.Serialize());

        string allocationID = RelayManager.Instance.GetAllocation();
        string connectionData = RelayManager.Instance.GetConnectionData();


        await LobbyControll.Instance.UpdatePlayerData(_localPlayerData.Id, _localPlayerData.Serialize(), allocationID, connectionData);
    
        await SceneManager.LoadSceneAsync(_lobbyData.SenceName);

        RelayServerData relayServerData = new RelayServerData(hostAllocation, "dtls"); 
       
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

       
        NetworkManager.Singleton.StartHost();
    }
}
