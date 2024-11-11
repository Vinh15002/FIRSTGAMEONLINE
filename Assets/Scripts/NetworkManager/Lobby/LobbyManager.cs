using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : SingletonPersistent<LobbyManager>
{

    [Space(10)]
    [Header("Lobby List")]
    [SerializeField]
    private GameObject showLobby;


    public static bool loadTheNextScene = false;



    [SerializeField] private Transform lobbyContentParent;
    [SerializeField] private Transform lobbyItemPrefab;

    public Lobby _lobby;
    public LobbyPlayerData _localPlayerData = new LobbyPlayerData();

    public LobbyData _lobbyData;


    private List<LobbyPlayerData> _lobbyPlayerDatas = new List<LobbyPlayerData>();

    public bool getnextScene = false;


    public string _lobbyID;
    public async void OnCreateLobby(){
        
        AudioManager.Instance.PlaySoundEffect();
        StartCoroutine(LoadingScene());
        bool suc = await CreateLobby();
        if(suc){
           
            //await Task.Delay(1000);
            
        }
        

        // Debug.Log(_lobby.MaxPlayers + " "  + _lobby.LobbyCode);
    }

    
    public async Task<bool> CreateLobby(){
        try{

            _lobbyData = new LobbyData();
            _lobbyData.MapIndex = 0;
            _lobbyData.SenceName = "MainSence";
            _lobbyData.RelayCode = "0";
            _lobbyData.Initialize(0);
            


            _localPlayerData = new LobbyPlayerData();
            _localPlayerData.Initialize(AuthenticationService.Instance.PlayerId, "HostPlayer");
            _localPlayerData.HasCodePlayer = 3269213171;
            Dictionary<string, PlayerDataObject> PlayerData = SerializePlayerData(_localPlayerData.Serialize());
            
            Player player = new Player(AuthenticationService.Instance.PlayerId, null, PlayerData);


            CreateLobbyOptions options = new CreateLobbyOptions{
                Data = SerilizeLobbyData(_lobbyData.Serialize()),
                IsPrivate = false,
                Player = player,

            };
            _lobby = await LobbyService.Instance.CreateLobbyAsync(
            "LOBBY", 4, options);
            _lobbyID = _lobby.Id;
            getnextScene = true;

            UpdateLobbyRepeated();
            LobbyHeartbeat(_lobby);
            
        }catch{
            return false;
        }
        
        
        return true;
    }



    public async void UpdateLobbyRepeated(){
        while (Application.isPlaying)
        {
            
            if (string.IsNullOrEmpty(_lobbyID))
            {
                return;
            }
            

            Lobby lobby = await Lobbies.Instance.GetLobbyAsync(_lobbyID);
            OnUpdateLobby(lobby);
            if(loadTheNextScene) return;
            await Task.Delay(1000);
            // if(_lobbyData.RelayCode != null){
            //     LoadingSceneJoin(_lobbyData.RelayCode);
            // }
            // if (!isJoined && lobby.Data["JoinCode"].Value != string.Empty)
            // {
            //     await relayManager.StartClientWithRelay(lobby.Data["JoinCode"].Value);
            //     isJoined = true;
            //     joinedLobbyParent.SetActive(false);
            //     return;
            // }

            // if (AuthenticationService.Instance.PlayerId == lobby.HostId)
            // {
            //     joinedLobbyStartButton.SetActive(true);
            // }
            // else
            // {
            //     joinedLobbyStartButton.SetActive(false);
            // }

            // joinedLobbyNameText.text = lobby.Name;
            // joinedLobbyGamemodeText.text = lobby.Data["GameMode"].Value;
            
            
        }
    }

    public async void LoadingSceneJoin(string relayCode)
    {
        await UpdateLobbyManager.Instance.JoinRelayPlay(relayCode);
    }



    private Dictionary<string, PlayerDataObject> SerializePlayerData(Dictionary<string, string> data)
    {
        Dictionary<string, PlayerDataObject> playerData = new Dictionary<string, PlayerDataObject>();

        foreach(var (key,value) in data ){
            playerData.Add(key, new PlayerDataObject(
                visibility:PlayerDataObject.VisibilityOptions.Public,
                value:value
            ));
        }
        return playerData;
    }




    public List<Dictionary<string, PlayerDataObject>> GetPlayerData(Lobby lobby)
    {
        List<Dictionary<string,PlayerDataObject>> data = new List<Dictionary<string, PlayerDataObject>>();
        foreach(Player player in lobby.Players){
            data.Add(player.Data);
        }
        return data;
    }

    public async void OnUpdateLobby(Lobby newLobby)
    {
        
        _lobby = newLobby;
        List<Dictionary<string,PlayerDataObject>> playerData = GetPlayerData(newLobby);
        _lobbyPlayerDatas.Clear();
        int count = 0;
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
        LobbyData lobbyData= new LobbyData();
        _lobbyID = newLobby.Id;
        lobbyData.Initialize(newLobby.Data);
        _lobbyData = lobbyData;
        
        if(_lobbyData.RelayCode.Length > 3 && !IsHost()){
            await UpdateLobbyManager.Instance.JoinRelayPlay(_lobbyData.RelayCode.ToString().Trim());
            return;
           
            
        }
       
        LobbySpawner.instance.OnLobbyUpdated(_lobbyPlayerDatas);
        
            

        LobbyUI.instance.SetMap(_lobbyData.MapIndex);
        if(count == _lobbyPlayerDatas.Count){
            LobbyUI.instance.SetStartButton(true);
        }
        else {
            LobbyUI.instance.SetStartButton(false);
        }
    }

    private async void LobbyHeartbeat(Lobby lobby){
        while(true){
            if(lobby == null){
                return;
            }

            await LobbyService.Instance.SendHeartbeatPingAsync(lobby.Id);

            await Task.Delay(15*1000);
        }
    }



    public async Task<bool> JoinLobby(string LobbyID){
        try{
            AudioManager.Instance.PlaySoundEffect();
            _localPlayerData = new LobbyPlayerData();
            _localPlayerData.Initialize(AuthenticationService.Instance.PlayerId, "JoinPlayer");

            _localPlayerData.HasCodePlayer = 3269213171;
            JoinLobbyByIdOptions options = new JoinLobbyByIdOptions();
            Dictionary<string, PlayerDataObject> PlayerData = SerializePlayerData(_localPlayerData.Serialize());
            Player player = new Player(AuthenticationService.Instance.PlayerId, null, PlayerData);
            options.Player = player;
            Lobby lobby = await LobbyService.Instance.JoinLobbyByIdAsync(LobbyID, options);
            
            _lobbyID = LobbyID;
            getnextScene = true;
            _lobby = lobby;
            
            
        }catch(LobbyServiceException e){
            return false;
        }
        UpdateLobbyRepeated();
        return true;
    }


    
    public List<LobbyPlayerData> GetPlayers()
    {
        return _lobbyPlayerDatas;
    }

    public void OnApplicationQuit() {
        if(_lobby != null && _lobby.HostId == AuthenticationService.Instance.PlayerId){
            LobbyService.Instance.DeleteLobbyAsync(_lobby.Id);
        }
        else if(_lobby != null) {
            ShowAllLobby();
        }
    }



    public Dictionary<string, DataObject> SerilizeLobbyData(Dictionary<string, string> lobbydata)
    {
        Dictionary<string, DataObject> lobbyData = new Dictionary<string, DataObject>();

        foreach(var (key,value) in lobbydata ){
            lobbyData.Add(key, new DataObject(
                visibility:DataObject.VisibilityOptions.Public,
                value:value
            ));
        }
        return lobbyData;
    }



    public async Task<bool> UpdateLobbyData(Dictionary<string, string> data)
    {
        Dictionary<string, DataObject> lobbyData = SerilizeLobbyData(data);
        UpdateLobbyOptions options = new UpdateLobbyOptions{
            Data = lobbyData
        };
        
        try {
            
            Lobby newLobby = await LobbyService.Instance.UpdateLobbyAsync(_lobby.Id, options);
           
            OnUpdateLobby(newLobby);

            
            
        }catch(SystemException){
            return false;
        }
        return true;

        
    }

    public async Task<bool> UpdatePlayerData(string id,Dictionary<string, string> dictionary)
    {
       
        Dictionary<string, PlayerDataObject> playerData = SerializePlayerData(dictionary);
        UpdatePlayerOptions options = new UpdatePlayerOptions{
            Data = playerData,
        };
        
        try {
            
            Lobby newLobby = await LobbyService.Instance.UpdatePlayerAsync(_lobby.Id,id, options);

            OnUpdateLobby(newLobby);
        }catch(SystemException){
            return false;
        }
        return true;
    }


    public IEnumerator LoadingScene(){
        
        
        //LoadingFadeEffect.Instance.FadeIn();
        yield return new WaitForSeconds(0f);
        
       
        LoadingFadeEffect.Instance.FadeOut();
        SceneManager.LoadSceneAsync("Lobby");

        getnextScene = true;

        
    }

    public IEnumerator LoadingScenceFade(){
        
        yield return new WaitForSeconds(1f);
        
        LoadingFadeEffect.Instance.FadeOut();
    }


    public bool IsHost(){
        return _localPlayerData.Id == _lobby.HostId;
        if(_lobbyPlayerDatas != null && _lobby != null){
            return _localPlayerData.Id == _lobby.HostId;
        }
        else return false;
        
    }


    
    public void ShowLobby(){
        
        showLobby.SetActive(true);
        ShowAllLobby();
        
    }

    

    private async void ShowAllLobby()
    {
        while(!getnextScene){
            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();
            if(lobbyContentParent==null) break;
            foreach (Transform t in lobbyContentParent){
                Destroy(t.gameObject);
            }

            foreach (Lobby lobby in queryResponse.Results){
                Transform newLobby = Instantiate(lobbyItemPrefab, lobbyContentParent);
                newLobby.GetChild(1).GetComponent<TextMeshProUGUI>().text = lobby.Name;
                newLobby.GetChild(2).GetComponent<TextMeshProUGUI>().text = lobby.Players.Count + "/" + lobby.MaxPlayers;
                newLobby.GetComponent<ButtonLobby>().lobbyID = lobby.Id;
                
            }
            await Task.Delay(1000);
        }
    }

    
}
