using System.Collections.Generic;
using UnityEngine;

public class LobbySpawner : MonoBehaviour {


    public static LobbySpawner instance;

    
    [SerializeField] private List<LobbyPlayer> _players;

    

    private void OnEnable() {
        LobbyEvents.lobbyUpdateUI += OnLoobyUpdated;
    }
    private void OnDisable(){
        LobbyEvents.lobbyUpdateUI -= OnLoobyUpdated;
    }



    public void OnLoobyUpdated(){
        List<LobbyPlayerData> playerDatas = GameLobbyController.Instance.GetPlayers();
        Debug.Log(playerDatas.Count);

        for(int i = 0; i < playerDatas.Count; i++){
            LobbyPlayerData data = playerDatas[i];
            Debug.Log("Player Ready: " + data.IsReady);
            _players[i].SetData(data);
        }

    }
}