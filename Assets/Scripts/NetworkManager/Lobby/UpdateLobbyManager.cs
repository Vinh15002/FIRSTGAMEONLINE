

using System;
using System.Collections;

using System.Diagnostics;
using System.Threading.Tasks;
using Unity.Netcode.Transports.UTP;
using Unity.Netcode;
using Unity.Networking.Transport.Relay;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

public class UpdateLobbyManager: SingletonPersistent<UpdateLobbyManager>{
    private void Start() {
        //StartCoroutine(UpdateLobby());
    }

    [SerializeField]
    private GameObject loadingScene;

    public Allocation createLobby;

    public JoinAllocation joinedLobby;


    public IEnumerator UpdateLobby(){
        yield return new WaitForSeconds(3f);
        while (true)
        {
            Task<Lobby> task = LobbyService.Instance.GetLobbyAsync(LobbyManager.Instance._lobbyID);
            yield return new WaitUntil(() => task.IsCompleted);
            Lobby newLobby = task.Result;
           
            LobbyManager.Instance.OnUpdateLobby(newLobby);
            
            yield return new WaitForSeconds(3f);
        }
        

    }

    public async Task<bool> SetSelectedMap(int currentMapIndex, string senceName)
    {
        LobbyData lobbyData = new LobbyData();
        lobbyData.MapIndex = currentMapIndex;
        lobbyData.SenceName = senceName;
        lobbyData.RelayCode = "0";
        


        return await LobbyManager.Instance.UpdateLobbyData(lobbyData.Serialize());
        
    }

    public async Task<bool> SetSelectedPlayer(int currenSelected, uint hasCode)
    {
        LobbyPlayerData lobbyPlayerData = LobbyManager.Instance._localPlayerData;
        lobbyPlayerData.IndexSelected = currenSelected;
        lobbyPlayerData.HasCodePlayer = hasCode;
        return await LobbyManager.Instance.UpdatePlayerData(lobbyPlayerData.Id,lobbyPlayerData.Serialize());
        
    }

    public async Task<bool> SetPlayerReady()
    {
        LobbyPlayerData lobbyPlayerData = LobbyManager.Instance._localPlayerData;
        lobbyPlayerData.IsReady = true;
        return await LobbyManager.Instance.UpdatePlayerData(lobbyPlayerData.Id,lobbyPlayerData.Serialize());
    }

    public async Task<bool> SetPlayerExit()
    {
        LobbyPlayerData lobbyPlayerData = LobbyManager.Instance._localPlayerData;
        lobbyPlayerData.IsReady = false;
        return await LobbyManager.Instance.UpdatePlayerData(lobbyPlayerData.Id,lobbyPlayerData.Serialize());
    }

    public async Task<bool> OnPlayerStart(){
        createLobby = await RelayManager.Instance.CreateRelay();
        LobbyData lobbyData = LobbyManager.Instance._lobbyData;
        lobbyData.RelayCode = RelayManager.Instance._joinCode;
        LobbyManager.loadTheNextScene= true;
        return await LobbyManager.Instance.UpdateLobbyData(lobbyData.Serialize());
    }

    public async Task JoinRelayPlay(string relayCode)
    {   if(loadingScene!=null)
            loadingScene.SetActive(true);
        joinedLobby = await RelayManager.Instance.JoinRelay(relayCode);
        if (joinedLobby != null)
        {
            LobbyManager.loadTheNextScene = true;
            StartCoroutine(LobbyManager.Instance.LoadingScenceFade());
            await SceneManager.LoadSceneAsync(LobbyManager.Instance._lobbyData.SenceName);
        }
       

    }
}